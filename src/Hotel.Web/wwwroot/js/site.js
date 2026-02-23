document.addEventListener('DOMContentLoaded', () => {
  setupCookieConsent();
  setupRoomFilters();
  setupGalleryFilters();
});

function setupCookieConsent() {
  const banner = document.getElementById('cookie-consent');
  const acceptButton = document.getElementById('cookie-consent-accept');
  if (!banner || !acceptButton) {
    return;
  }

  const key = 'cookieConsent44Life';
  if (localStorage.getItem(key) === 'accepted') {
    banner.hidden = true;
    return;
  }

  banner.hidden = false;
  acceptButton.addEventListener('click', () => {
    localStorage.setItem(key, 'accepted');
    banner.hidden = true;
  });
}

function setupRoomFilters() {
  const filterRoot = document.querySelector('[data-room-filters]');
  const cards = Array.from(document.querySelectorAll('[data-room-item]'));
  const emptyMessage = document.querySelector('[data-room-empty]');

  if (!filterRoot || cards.length === 0) {
    return;
  }

  const controls = {
    capacity: filterRoot.querySelector('[data-filter="capacity"]'),
    minSize: filterRoot.querySelector('[data-filter="min-size"]'),
    view: filterRoot.querySelector('[data-filter="view"]'),
    bed: filterRoot.querySelector('[data-filter="bed"]'),
    pool: filterRoot.querySelector('[data-filter="pool"]')
  };

  const applyFilters = () => {
    let visibleCount = 0;

    cards.forEach((card) => {
      const capacityValue = Number(card.getAttribute('data-capacity') || '0');
      const sizeValue = Number(card.getAttribute('data-size') || '0');
      const viewValue = (card.getAttribute('data-view') || '').toLowerCase();
      const bedValue = (card.getAttribute('data-bed') || '').toLowerCase();
      const poolValue = (card.getAttribute('data-pool') || '').toLowerCase();

      const capacityFilter = Number(controls.capacity?.value || '0');
      const minSizeFilter = Number(controls.minSize?.value || '0');
      const viewFilter = (controls.view?.value || '').toLowerCase();
      const bedFilter = (controls.bed?.value || '').toLowerCase();
      const poolFilter = (controls.pool?.value || '').toLowerCase();

      const matches =
        (capacityFilter === 0 || capacityValue >= capacityFilter) &&
        (minSizeFilter === 0 || sizeValue >= minSizeFilter) &&
        (!viewFilter || viewValue === viewFilter) &&
        (!bedFilter || bedValue === bedFilter) &&
        (!poolFilter || poolValue === poolFilter);

      card.classList.toggle('d-none', !matches);
      if (matches) {
        visibleCount += 1;
      }
    });

    if (emptyMessage) {
      emptyMessage.classList.toggle('d-none', visibleCount > 0);
    }
  };

  Object.values(controls).forEach((control) => {
    if (control) {
      control.addEventListener('change', applyFilters);
    }
  });
}

function setupGalleryFilters() {
  const buttonRoot = document.querySelector('[data-gallery-filters]');
  const items = Array.from(document.querySelectorAll('[data-gallery-item]'));
  if (!buttonRoot || items.length === 0) {
    return;
  }

  const buttons = Array.from(buttonRoot.querySelectorAll('[data-gallery-filter]'));
  buttons.forEach((button) => {
    button.addEventListener('click', () => {
      const value = (button.getAttribute('data-gallery-filter') || 'all').toLowerCase();

      buttons.forEach((otherButton) => otherButton.classList.remove('active'));
      button.classList.add('active');

      items.forEach((item) => {
        const category = (item.getAttribute('data-category') || '').toLowerCase();
        const isVisible = value === 'all' || category === value;
        item.classList.toggle('d-none', !isVisible);
      });
    });
  });
}
