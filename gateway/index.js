const fs = require('fs');
const wppconnect = require('@wppconnect-team/wppconnect');
const express = require('express');
const bodyParser = require('body-parser');
const axios = require('axios');

const app = express();
const nodeServicePort = 3020;

app.use(bodyParser.json());

let wppClientInstance;

const API_INCOMING_MESSAGE_WEBHOOK_URL = 'http://localhost:5256/api/whatsapp/webhook/message-received';

async function sendToApi(webhookType, data) {
  try {
    await axios.post(API_INCOMING_MESSAGE_WEBHOOK_URL, { webhookType, ...data });
  } catch (error) {
    console.error(`Erro ao enviar webhook '${webhookType}' para API:`, error.message);
  }
}

app.post('/api/whatsapp/send-message', async (req, res) => {
  if (!wppClientInstance) {
    return res.status(503).json({ error: 'Cliente não inicializado' });
  }

  const { recipientNumber, messageText } = req.body;
  if (!recipientNumber || !messageText) {
    return res.status(400).json({ error: 'Os parâmetros recipientNumber e messageText são obrigatórios.' });
  }

  try {
    const formattedRecipientNumber = recipientNumber.includes('@c.us') ? recipientNumber : `${recipientNumber}@c.us`;
    const result = await wppClientInstance.sendText(formattedRecipientNumber, messageText);

    res.status(200).json({ success: true, messageId: result.id.id, statusMessage: 'Mensagem enviada.' });
  } catch (error) {
    console.error(`Erro ao enviar mensagem para ${recipientNumber}:`, error.message);
    res.status(500).json({ error: 'Erro interno (500) ao enviar mensagem.', details: error.message });
  }
});

wppconnect
  .create({
    session: 'radarSolidarioNodeService',
    catchQR: (base64Qr, asciiQR) => {
      console.log(asciiQR);
      const matches = base64Qr.match(/^data:([A-Za-z-+\/]+);base64,(.+)$/);
      if (matches && matches.length === 3) {
        const imageBuffer = Buffer.from(matches[2], 'base64');
        fs.writeFile('qrcode_wpp_service.png', imageBuffer, 'binary', (err) => {
          if (err) console.error('Erro ao salvar QR Code:', err.message);
          else console.log('QR Code salvo como qrcode_wpp_service.png');
        });
      }
    },
    logQR: true,
    statusFind: (statusSession, session) => {
      console.log(`Status da sessão: ${statusSession} (${session})`);
    },
    headless: 'new',
    devtools: false,
    puppeteerOptions: {
      args: ['--no-sandbox', '--disable-setuid-sandbox']
    },
    disableWelcome: true,
  })
  .then((client) => {
    wppClientInstance = client;
    initializeWppListeners(client);

    app.listen(nodeServicePort, () => {
      console.log(`Serviço Node.js rodando na porta ${nodeServicePort}`);
      console.log(`Endpoint para enviar mensagens: POST http://localhost:${nodeServicePort}/api/whatsapp/send-message`);
    });
  })
  .catch((error) => {
    console.error('Erro crítico ao criar sessão WPPConnect:', error.message);
  });

function initializeWppListeners(client) {
  client.onMessage(async (message) => {
    if (message.isGroupMsg || message.isStatusMsg || message.fromMe) {
      return;
    }
    console.log(`Mensagem recebida de ${message.from}: "${message.body}"`);

    await sendToApi('newWhatsappMessage', {
      sender: message.from,
      messageBody: message.body,
      timestamp: message.timestamp,
      messageId: message.id,
    });
  });

  client.onStateChange((state) => {
    console.log(`Estado do cliente alterado: ${state}`);
    if (state === 'CONFLICT' || state === 'UNLAUNCHED' || state === 'UNPAIRED') {
      console.log('Cliente desconectado ou em estado de conflito. Tentando fechar...');
      try {
        client.close();
      } catch (e) {
        console.error('Erro ao fechar cliente em estado de conflito:', e.message);
      }
      wppClientInstance = null;
    }
  });
}

process.on('SIGINT', async () => {
  console.log('Encerrando serviço...');
  if (wppClientInstance) {
    try {
      await wppClientInstance.close();
      console.log('Cliente desconectado.');
    } catch (e) {
      console.error('Erro ao desconectar cliente:', e.message);
    }
  }
  process.exit(0);
});
