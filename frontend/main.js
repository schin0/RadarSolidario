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
document.querySelectorAll('#mobile-menu a').forEach(link => {
  link.addEventListener('click', () => {
    mobileMenu.classList.add('hidden');
  });
});

const stepItems = document.querySelectorAll('.step-item');
stepItems.forEach(item => {
  item.addEventListener('click', () => {
    stepItems.forEach(otherItem => {
      if (otherItem !== item && otherItem.classList.contains('step-active')) {
        otherItem.classList.remove('step-active');
      }
    });
    item.classList.toggle('step-active');
  });
});

const sectionsToReveal = document.querySelectorAll('.section-reveal');
const revealObserver = new IntersectionObserver((entries, observer) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.classList.add('visible');
      observer.unobserve(entry.target);
    }
  });
}, { threshold: 0.1 });

sectionsToReveal.forEach(section => {
  revealObserver.observe(section);
});

const heroTitle = document.querySelector('.animate-fade-in-down');
const heroText = document.querySelectorAll('.animate-fade-in-up');
