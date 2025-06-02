const header = document.getElementById('header');
window.addEventListener('scroll', () => {
  if (window.scrollY > 50) {
    header.classList.remove('bg-transparent');
    header.classList.add('bg-sky-700', 'shadow-lg');
  } else {
    header.classList.remove('bg-sky-700', 'shadow-lg');
    header.classList.add('bg-transparent');
  }
});

const mobileMenuButton = document.getElementById('mobile-menu-button');
const mobileMenu = document.getElementById('mobile-menu');
mobileMenuButton.addEventListener('click', () => {
  mobileMenu.classList.toggle('hidden');
});
document.querySelectorAll('#mobile-menu a, #header nav a').forEach(link => {
  link.addEventListener('click', (e) => {
    if (link.getAttribute('href').startsWith('#')) {
      mobileMenu.classList.add('hidden');
    }
  });
});

const stepItems = document.querySelectorAll('.step-item');
stepItems.forEach(item => {
  item.addEventListener('click', () => {
    const currentlyActive = document.querySelector('.step-item.step-active');
    if (currentlyActive && currentlyActive !== item) {
      currentlyActive.classList.remove('step-active');
    }
    item.classList.toggle('step-active');
  });
});

const impactSection = document.getElementById('nosso-impacto');
const loadingStatsElement = document.getElementById('loading-stats');
let statsFetched = false;

async function fetchImpactNumbers() {
  if (statsFetched)
    return;

  statsFetched = true;

  const apiUrl = 'http://localhost:5256/api/Info';

  try {
    const response = await fetch(apiUrl);
    if (!response.ok) {
      throw new Error(`Erro HTTP: ${response.status}`);
    }
    const data = await response.json();

    document.getElementById('impactPessoasAjudadas')?.setAttribute('data-count', data.quantityHelp || 0);
    document.getElementById('impactVoluntarios')?.setAttribute('data-count', data.quantityVolunteers || 0);
    document.getElementById('impactConexoes')?.setAttribute('data-count', data.connections || 0);
    document.getElementById('impactComunidades')?.setAttribute('data-count', data.communitiesServed || 0);

    if (loadingStatsElement) loadingStatsElement.style.display = 'none';

    animateCounters();
  } catch (error) {
    if (loadingStatsElement) loadingStatsElement.textContent = 'Não foi possível carregar as estatísticas.';
  }
}

function animateCounters() {
  const counters = document.querySelectorAll('.impact-number');
  const speed = 200;

  counters.forEach(counter => {
    if (counter.classList.contains('animated')) return;
    counter.classList.add('animated');
    counter.innerText = '0';

    const updateCount = () => {
      const target = +counter.getAttribute('data-count');
      const count = +counter.innerText.replace('+', '');
      const increment = target / speed;

      if (count < target) {
        counter.innerText = Math.ceil(count + increment);
        setTimeout(updateCount, 15);
      } else {
        let finalTargetText = target.toLocaleString('pt-BR');
        counter.innerText = finalTargetText;
      }
    };
    updateCount();
  });
}

const sectionsToReveal = document.querySelectorAll('.section-reveal');
const revealObserverOptions = { threshold: 0.1 };
const revealObserver = new IntersectionObserver((entries, observer) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.classList.add('visible');
      if (entry.target.id === 'nosso-impacto' && !statsFetched) {
        fetchImpactNumbers();
      }
      observer.unobserve(entry.target);
    }
  });
}, revealObserverOptions);

sectionsToReveal.forEach(section => {
  revealObserver.observe(section);
});

const eventosCtx = document.getElementById('eventosClimaticosChart');
if (eventosCtx) {
  new Chart(eventosCtx, {
    type: 'bar',
    data: {
      labels: ['2022', '2023', '2024', '2025 (Proj.)'],
      datasets: [{
        label: 'Eventos Climáticos Graves (Brasil)',
        data: [1050, 1161, 1690, 1820],
        backgroundColor: 'rgba(2, 132, 199, 0.7)',
        borderColor: 'rgba(2, 132, 199, 1)',
        borderWidth: 1,
        borderRadius: 4,
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      scales: {
        y: {
          beginAtZero: true,
          grid: { display: false },
          ticks: { font: { size: 10, family: 'Inter' }, color: '#475569' }
        },
        x: {
          grid: { display: false },
          ticks: { font: { size: 12, family: 'Inter', weight: '600' }, color: '#334155' }
        }
      },
      plugins: {
        legend: { display: false },
        title: {
          display: true,
          text: 'Eventos Climáticos Impactantes no Brasil (CEMADEN)',
          font: { size: 14, family: 'Lexend', weight: '600' },
          color: '#1e293b',
          padding: { bottom: 10 }
        },
        tooltip: {
          callbacks: {
            label: function (context) {
              let label = context.dataset.label || '';
              if (label) {
                label += ': ';
              }
              if (context.parsed.y !== null) {
                label += context.parsed.y.toLocaleString('pt-BR') + ' eventos';
              }
              return label;
            }
          }
        }
      }
    }
  });
}

const tiposAjudaCtx = document.getElementById('tiposAjudaChart');
if (tiposAjudaCtx) {
  new Chart(tiposAjudaCtx, {
    type: 'doughnut',
    data: {
      labels: ['Abrigo', 'Alimentos/Água', 'Resgate Leve', 'Transporte', 'Outros'],
      datasets: [{
        label: 'Tipos de Necessidades Atendidas',
        data: [35, 25, 20, 15, 5],
        backgroundColor: [
          'rgba(14, 165, 233, 0.8)',
          'rgba(16, 185, 129, 0.8)',
          'rgba(245, 158, 11, 0.8)',
          'rgba(99, 102, 241, 0.8)',
          'rgba(107, 114, 128, 0.8)'
        ],
        borderColor: '#fff',
        borderWidth: 2,
        hoverOffset: 8
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            font: { size: 10, family: 'Inter' },
            color: '#f1f5f9',
            padding: 10,
            boxWidth: 12
          }
        },
        title: {
          display: false
        },
        tooltip: {
          callbacks: {
            label: function (context) {
              let label = context.label || '';
              if (label) {
                label += ': ';
              }
              if (context.parsed !== null) {
                label += context.parsed + '%';
              }
              return label;
            }
          }
        }
      },
      cutout: '60%'
    }
  });
}
