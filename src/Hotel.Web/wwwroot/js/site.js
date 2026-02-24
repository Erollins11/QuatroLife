document.addEventListener('DOMContentLoaded', () => {
  setupCookieConsent();
  setupAvailabilityEstimator();
  setupRoomFilters();
  setupRoomComparison();
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

function setupAvailabilityEstimator() {
  const widget = document.querySelector('[data-availability-widget]');
  if (!widget) {
    return;
  }

  const roomSelect = widget.querySelector('[data-availability-room]');
  const checkInInput = widget.querySelector('[data-availability-checkin]');
  const checkOutInput = widget.querySelector('[data-availability-checkout]');
  const guestSelect = widget.querySelector('[data-availability-guests]');
  const totalOutput = widget.querySelector('[data-availability-total]');
  if (!roomSelect || !checkInInput || !checkOutInput || !guestSelect || !totalOutput) {
    return;
  }

  const locale = document.documentElement.lang === 'en' ? 'en-US' : 'tr-TR';

  const pad = (value) => value.toString().padStart(2, '0');
  const toIsoDate = (date) => `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
  const parseDate = (value) => {
    const parsed = new Date(`${value}T00:00:00`);
    return Number.isNaN(parsed.getTime()) ? null : parsed;
  };

  const today = new Date();
  const minCheckIn = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 1);
  const defaultCheckOut = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 3);

  checkInInput.min = toIsoDate(minCheckIn);
  checkInInput.value = toIsoDate(minCheckIn);
  checkOutInput.min = toIsoDate(new Date(minCheckIn.getFullYear(), minCheckIn.getMonth(), minCheckIn.getDate() + 1));
  checkOutInput.value = toIsoDate(defaultCheckOut);

  const formatPrice = (amount) => new Intl.NumberFormat(locale, { maximumFractionDigits: 0 }).format(Math.round(amount));

  const updateEstimate = () => {
    const selectedOption = roomSelect.options[roomSelect.selectedIndex];
    const basePrice = Number(selectedOption?.dataset.price || '0');
    const checkInDate = parseDate(checkInInput.value);
    const checkOutDate = parseDate(checkOutInput.value);
    const guests = Number(guestSelect.value || '2');

    if (!checkInDate || !checkOutDate || checkOutDate <= checkInDate || basePrice <= 0) {
      totalOutput.textContent = '0';
      return;
    }

    const oneDayMs = 1000 * 60 * 60 * 24;
    const nights = Math.max(1, Math.round((checkOutDate.getTime() - checkInDate.getTime()) / oneDayMs));
    const guestMultiplier = guests <= 2 ? 1 : 1 + (guests - 2) * 0.12;
    const estimate = basePrice * nights * guestMultiplier;
    totalOutput.textContent = formatPrice(estimate);
  };

  checkInInput.addEventListener('change', () => {
    const selectedCheckIn = parseDate(checkInInput.value);
    if (!selectedCheckIn) {
      return;
    }
    const minCheckOut = new Date(selectedCheckIn.getFullYear(), selectedCheckIn.getMonth(), selectedCheckIn.getDate() + 1);
    checkOutInput.min = toIsoDate(minCheckOut);
    const selectedCheckOut = parseDate(checkOutInput.value);
    if (!selectedCheckOut || selectedCheckOut <= selectedCheckIn) {
      checkOutInput.value = toIsoDate(minCheckOut);
    }
    updateEstimate();
  });

  [roomSelect, checkOutInput, guestSelect].forEach((control) => control.addEventListener('change', updateEstimate));
  updateEstimate();
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

function setupRoomComparison() {
  const toggles = Array.from(document.querySelectorAll('[data-room-compare-toggle]'));
  const compareBar = document.querySelector('[data-room-compare-bar]');
  const compareList = document.querySelector('[data-room-compare-list]');
  const compareTable = document.querySelector('[data-room-compare-table]');
  const clearButton = document.querySelector('[data-room-compare-clear]');
  const openButton = document.querySelector('[data-room-compare-open]');
  if (toggles.length === 0 || !compareBar || !compareList || !compareTable || !clearButton || !openButton) {
    return;
  }

  const maxItems = 3;
  const currency = compareBar.getAttribute('data-currency') || '';
  const detailsLabel = compareBar.getAttribute('data-details-label') || 'Details';
  const maxMessage = compareBar.getAttribute('data-max-message') || '';
  const selectedRooms = new Map();

  const createRoomPayload = (toggle) => ({
    id: toggle.getAttribute('data-room-id') || '',
    title: toggle.getAttribute('data-room-title') || '',
    size: toggle.getAttribute('data-room-size') || '',
    capacity: toggle.getAttribute('data-room-capacity') || '',
    view: toggle.getAttribute('data-room-view') || '',
    bed: toggle.getAttribute('data-room-bed') || '',
    pool: toggle.getAttribute('data-room-pool') || '',
    price: Number(toggle.getAttribute('data-room-price') || '0'),
    url: toggle.getAttribute('data-room-url') || '#'
  });

  const formatPrice = (amount) => {
    const locale = document.documentElement.lang === 'en' ? 'en-US' : 'tr-TR';
    return `${new Intl.NumberFormat(locale, { maximumFractionDigits: 0 }).format(amount)} ${currency}`.trim();
  };

  const renderCompareBar = () => {
    compareList.innerHTML = '';
    selectedRooms.forEach((room) => {
      const chip = document.createElement('span');
      chip.className = 'room-compare-chip';
      chip.textContent = room.title;
      compareList.appendChild(chip);
    });
    compareBar.classList.toggle('d-none', selectedRooms.size === 0);
    openButton.disabled = selectedRooms.size < 2;
  };

  const renderCompareTable = () => {
    compareTable.innerHTML = '';
    selectedRooms.forEach((room) => {
      const row = document.createElement('tr');
      row.innerHTML = `
        <td>${room.title}</td>
        <td>${formatPrice(room.price)}</td>
        <td>${room.size} m²</td>
        <td>${room.capacity}</td>
        <td>${room.view}</td>
        <td>${room.bed}</td>
        <td>${room.pool}</td>
        <td><a class="btn btn-outline-dark btn-sm" href="${room.url}">${detailsLabel}</a></td>
      `;
      compareTable.appendChild(row);
    });
  };

  toggles.forEach((toggle) => {
    toggle.addEventListener('change', () => {
      const room = createRoomPayload(toggle);
      if (toggle.checked) {
        if (selectedRooms.size >= maxItems) {
          toggle.checked = false;
          if (maxMessage) {
            window.alert(maxMessage);
          }
          return;
        }
        selectedRooms.set(room.id, room);
      } else {
        selectedRooms.delete(room.id);
      }
      renderCompareBar();
    });
  });

  clearButton.addEventListener('click', () => {
    selectedRooms.clear();
    toggles.forEach((toggle) => {
      toggle.checked = false;
    });
    renderCompareBar();
    compareTable.innerHTML = '';
  });

  openButton.addEventListener('click', renderCompareTable);
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
