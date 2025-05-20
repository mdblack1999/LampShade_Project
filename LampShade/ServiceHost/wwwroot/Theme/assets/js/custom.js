const cookieName = "cart-items";

// خواندن و نوشتن کوکی
function getCartProducts() {
    const raw = $.cookie(cookieName);
    try { return raw ? JSON.parse(raw) : []; }
    catch { return []; }
}
function saveCartProducts(products) {
    $.cookie(cookieName, JSON.stringify(products), {
        expires: 2,
        path: "/"
    });
}

// اضافه کردن به سبد
function addToCart(id, name, price, picture) {
    let products = getCartProducts();
    const count = parseInt($("#productCount").val()) || 1;
    const existing = products.find(x => x.id === id);

    if (existing) {
        existing.count = parseInt(existing.count) + count;
    } else {
        products.push({ id, name, unitPrice: price, picture, count });
    }

    saveCartProducts(products);
    updateCart();

    // Effect
}

// حذف آیتم
function removeFromCart(id) {
    let products = getCartProducts();
    const idx = products.findIndex(x => x.id === id);
    if (idx > -1) {
        products.splice(idx, 1);
        saveCartProducts(products);
        updateCart();
    }
}

// آپدیت همزمان MiniCart و  Cart
function updateCart() {
    const products = getCartProducts();

    // a) MiniCart
    $("#cart_items_count").text(products.length);
    const miniWrap = $("#cart_items_wrapper").empty();
    products.forEach(x => {
        miniWrap.append(`
      <div class="single-cart-item">
        <a href="javascript:void(0)" class="remove-icon" onclick="removeFromCart('${x.id}')">
          <i class="ion-android-close"></i>
        </a>
        <div class="image">
          <img src="/ProductPictures/${x.picture}" class="img-fluid" alt="">
        </div>
        <div class="content">
          <p class="product-title">
            <a href="single-product.html">محصول: ${x.name}</a>
          </p>
          <p class="count">تعداد: ${x.count} عدد</p>
          <p class="count">قیمت واحد: ${Number(x.unitPrice).toLocaleString()} تومان</p>
        </div>
      </div>
    `);
    });

    // b) Cart
    const fullWrap = $("#full_cart_wrapper").empty();
    products.forEach(x => {
        fullWrap.append(`
      <tr>
        <td class="pro-thumbnail">
          <a href="single-product.html">
            <img src="/ProductPictures/${x.picture}" class="img-fluid" alt="${x.name}">
          </a>
        </td>
        <td class="pro-title">
          <a href="single-product.html" style="font-size: 18px;">${x.name}</a>
        </td>
        <td class="pro-price">
          <span>${Number(x.unitPrice).toLocaleString()}</span>
        </td>
        <td class="pro-quantity">
          <div class="quantity-selection">
            <input type="number"
                   value="${x.count}"
                   min="1"
                   onchange="changeCartItemCount('${x.id}', 'totalItemPrice-${x.id}', this.value)" />
          </div>
        </td>
        <td class="pro-subtotal">
          <span id="totalItemPrice-${x.id}">
            ${(x.count * x.unitPrice).toLocaleString()}
          </span>
        </td>
        <td class="pro-remove">
          <a href="javascript:void(0)" onclick="removeFromCart('${x.id}')">
            <i class="fa fa-trash-o text-danger" style="font-size: 28px;"></i>
          </a>
        </td>
      </tr>
    `);
    });
}

// تغییر تعدادِ یک آیتم و بروز رسانی UI
function changeCartItemCount(id, totalId, count) {
    let products = getCartProducts();
    products = JSON.parse(JSON.stringify(products));
    const idx = products.findIndex(x => x.id === id);

    if (idx > -1) {
        products[idx].count = parseInt(count) || 1;
        const unit = Number(products[idx].unitPrice);
        const subtotal = unit * products[idx].count;

        // 1) به‌روزرسانی نقطه‌ای subtotal
        $(`#${totalId}`).text(subtotal.toLocaleString());

        // 2) ذخیره در کوکی
        saveCartProducts(products);

        window.location.reload();
    }
}
$(document).ready(function () {
    updateCart();
});
