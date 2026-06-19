let selectedOrderId = null;

const els = {
    ordersList: document.getElementById('orders-list'),
    count: document.getElementById('orders-count'),
    detailsCol: document.getElementById('order-details-col'),
    info: document.getElementById('order-info'),
    stateDiagram: document.getElementById('order-state-diagram'),
    actions: document.getElementById('order-actions'),
    history: document.getElementById('order-history'),
    modal: document.getElementById('modal'),
    toast: document.getElementById('toast'),
    toastMsg: document.getElementById('toast-msg')
};


async function fetchOrders() {
    try {
        const res = await fetch('/orders');
        const orders = await res.json();
        renderOrdersList(orders);
    } catch (e) { showToast('Eroare la încărcarea comenzilor', true); }
}

async function fetchOrder(id) {
    try {
        const res = await fetch(`/orders/${id}`);
        if (!res.ok) throw new Error();
        const order = await res.json();
        selectedOrderId = id;
        renderOrderDetails(order);
    } catch (e) { showToast('Eroare la încărcarea detaliilor', true); }
}

async function createOrder() {
    const data = {
        name: document.getElementById('new-name').value,
        email: document.getElementById('new-email').value,
        age: parseInt(document.getElementById('new-age').value),
        isTrusted: document.getElementById('new-trusted').checked,
        street: "Str. Test", city: "Bucuresti", postalCode: "010", country: "RO",
        items: [{
            productId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            productName: "Laptop", quantity: 1, unitPrice: 5000, hasAgeRestriction: false
        }]
    };

    const res = await fetch('/orders', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });

    if (res.ok) {
        showToast('Comandă creată cu succes!', false);
        els.modal.style.display = 'none';
        fetchOrders();
    } else {
        const err = await res.json();
        showToast(err.error || 'Eroare validare', true);
    }
}

async function triggerAction(action) {
    if (!selectedOrderId) return;

    const res = await fetch(`/orders/${selectedOrderId}/${action}`, { method: 'POST' });
    if (res.ok) {
        showToast(`Acțiunea '${action}' a reușit!`, false);
        const updatedOrder = await res.json();
        renderOrderDetails(updatedOrder);
        fetchOrders();
    } else {
        const err = await res.text();
        showToast(JSON.parse(err).detail || err || 'Tranziție invalidă', true);
    }
}


function renderOrdersList(orders) {
    els.count.innerText = `· ${orders.length}`;
    els.ordersList.innerHTML = orders.map(o => `
        <div class="spa-order-row ${o.id.value === selectedOrderId ? 'active' : ''}" onclick="fetchOrder('${o.id.value}')">
            <span>${o.id.value === selectedOrderId ? '▶' : ''}</span>
            <span style="font-family: monospace; color: #58a6ff;">#${o.id.value.substring(0, 6)}</span>
            <span class="ost ${o.status}">${o.status}</span>
        </div>
    `).join('');
}

function renderOrderDetails(order) {
    els.detailsCol.style.visibility = 'visible';

    els.info.innerHTML = `
        <dt>ID</dt><dd style="font-family:monospace; color:#58a6ff">${order.id.value}</dd>
        <dt>Status</dt><dd>${order.status}</dd>
        <dt>Client</dt><dd>${order.customer.name} (${order.customer.age} ani)</dd>
        <dt>Total</dt><dd>${order.total.amount} ${order.total.currency}</dd>
    `;

    const states = ['Pending', 'Confirmed', 'Processing', 'Shipped', 'Delivered'];
    const currentIndex = states.indexOf(order.status);

    if (order.status === 'Cancelled') {
        els.stateDiagram.innerHTML = `<span class="spa-mini-node current" style="color:red; border-color:red; background:rgba(248,81,73,.1)">Cancelled</span>`;
    } else {
        els.stateDiagram.innerHTML = states.map((s, i) => `
            <span class="spa-mini-node ${i < currentIndex ? 'done' : ''} ${i === currentIndex ? 'current' : ''}">${s}</span>
            ${i < states.length - 1 ? '<span style="color:#6e7681; font-size:10px;">→</span>' : ''}
        `).join('');
    }

    const canPay = order.status === 'Pending';
    const canProcess = order.status === 'Confirmed';
    const canShip = order.status === 'Processing';
    const canDeliver = order.status === 'Shipped';
    const canCancel = ['Pending', 'Confirmed', 'Processing'].includes(order.status);

    els.actions.innerHTML = `
        <button class="spa-action ${canPay ? 'enabled' : 'disabled'}" onclick="${canPay ? "triggerAction('pay')" : ""}">Pay</button>
        <button class="spa-action ${canProcess ? 'enabled' : 'disabled'}" onclick="${canProcess ? "triggerAction('process')" : ""}">Process</button>
        <button class="spa-action ${canShip ? 'enabled' : 'disabled'}" onclick="${canShip ? "triggerAction('ship')" : ""}">Ship</button>
        <button class="spa-action ${canDeliver ? 'enabled' : 'disabled'}" onclick="${canDeliver ? "triggerAction('deliver')" : ""}">Deliver</button>
        <button class="spa-action ${canCancel ? 'cancel' : 'disabled'}" onclick="${canCancel ? "triggerAction('cancel')" : ""}">Cancel</button>
    `;

    els.history.innerHTML = order.history.map(h => `
        <div class="spa-history-row">
            <span style="color:#3fb950">●</span>
            <span style="flex:1; font-family:monospace;">${h.fromState} → ${h.toState}</span>
            <span style="color:#6e7681">${new Date(h.at).toLocaleTimeString()}</span>
        </div>
    `).join('');
}


function showToast(msg, isError) {
    els.toast.className = `spa-toast ${isError ? 'error' : 'success'}`;
    els.toastMsg.innerText = msg;
    els.toast.style.display = 'flex';
    setTimeout(() => { els.toast.style.display = 'none'; }, 4000);
}

document.getElementById('btn-new-order').onclick = () => els.modal.style.display = 'flex';
document.getElementById('btn-cancel').onclick = () => els.modal.style.display = 'none';
document.getElementById('btn-submit').onclick = createOrder;
document.getElementById('toast-close').onclick = () => els.toast.style.display = 'none';

fetchOrders();