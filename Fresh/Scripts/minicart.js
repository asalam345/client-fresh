function containsItem(id) {
    debugger;
    if (localStorage.length == 0) {
            return null;
        }

        for (i = 0; i <= localStorage.length - 1; i++) {

            var key = localStorage.key(i);

        if (id == key) {
            return localStorage.getItem(key);
        }

    }
    return null;

}
function CartListFunction() {
    debugger;
    $('.value-plus').on('click', function () {
        var tr = $(this).closest("tr");
        var id = $(tr).attr("data-id");
        var item = AddToCart(id, null, null, 1, 1, 1, 1, null, '+');
     if (item != -1) {
         var divUpd = $(this).parent().find('.value'), newVal = parseInt(divUpd.text(), 10) + 1;
         divUpd.text(newVal);
        }
    });

    $('.value-minus').on('click', function () {
        var tr = $(this).closest("tr");
        var id = $(tr).attr("data-id");
        var item = AddToCart(id, null, null, 1, 1, 1, 1, null, '-');
      //  alert(item);
        if (item == 0) {
            $(tr).remove();
            RemoveItem(id);
        }
        else if (item != -1) {
            var divUpd = $(this).parent().find('.value'), newVal = parseInt(divUpd.text(), 10) - 1;
            if (newVal >= 1) divUpd.text(newVal);
        }
    });
    $('.deleteCart').on('click', function (c) {
        var tr = $(this).closest("tr");
        var id = $(tr).attr("data-id");
        $(tr).fadeOut('slow', function (c) {
            $(tr).remove();
            RemoveItem(id);
        });
    });
}
function AddToCartIncriDicri(id, obj, incOrDic) {
    debugger;
    if (obj == null) {
        obj = JSON.parse(containsItem(id));
    }

    var modifiedQuantity = 0;
    if (incOrDic == '+') {
        modifiedQuantity = parseInt(obj.quantity + 1);
    }
    else {
        if (obj.quantity > 1) {
            modifiedQuantity = parseInt(obj.quantity - 1);
        }
        else {
            
            return 0;
        }
    }
    if (modifiedQuantity > obj.availableQuantity) {
        alert('Not available!');
        return -1;
    }
    else {
       var item = {
            name: obj.name,
            price: obj.price,
            id: obj.id,
            pic: obj.pic,
            quantity: modifiedQuantity,
            discount: obj.discount,
            total: parseFloat(obj.price) * parseInt(modifiedQuantity),
            totalDiscount: parseFloat(obj.discount) * parseInt(modifiedQuantity),
            availableQuantity: obj.availableQuantity
       };

       return item;
    }
}
function AddToCart(id, name, pic, quantity, price, discount, availableQuantity, customer, incOrDic) {
    debugger;
    var patt = new RegExp("^[1-9]([0-9]+)?$");
    var priceRegExp = new RegExp("^([0-9]+)(([.]+)([0-9]+)+)?$");
    //count.value = (patt.test(count.value) === true) ? parseInt(count.value) : 1;
    //discount = parseFloat(discount);
    //alert(priceRegExp.test(discount));
    //alert(priceRegExp.test(price));
    var item = [];
    if (patt.test(id) === true && patt.test(quantity) === true && patt.test(availableQuantity) === true && quantity <= availableQuantity
         && priceRegExp.test(discount) && priceRegExp.test(price)) {
        var obj = containsItem(id);
        //alert(obj);
        if (obj === undefined || obj == null) {
            //alert('eee');
            item = {
                name: name,
                price: price,
                id: id,
                pic: pic,
                quantity: quantity,
                discount: discount,
                total: parseFloat(price) * parseInt(quantity),
                totalDiscount: discount,
                availableQuantity: availableQuantity
            };
    }
    else {
        obj = JSON.parse(obj);
        item = AddToCartIncriDicri(id, obj, incOrDic);
    }
        var stringified = JSON.stringify(item);
        if (stringified == "0") {
            return 0;
        }
        else if (stringified == "-1") {
            return -1;
        }
        localStorage.setItem(id, stringified);
    }
    
   
    doShowAll();
}
//------------------------------------------------------------------------------
//change an existing key=>value in the HTML5 storage
//function ModifyItem() {
//    var name1 = document.forms.ShoppingList.name.value;
//    var data1 = document.forms.ShoppingList.data.value;
//    //check if name1 is already exists

//    //check if key exists
//    if (localStorage.getItem(name1) != null) {
//        //update
//        localStorage.setItem(name1, data1);
//        document.forms.ShoppingList.data.value = localStorage.getItem(name1);
//    }


//    doShowAll();
//}
//-------------------------------------------------------------------------
//delete an existing key=>value from the HTML5 storage
function RemoveItem(id) {
    localStorage.removeItem(id);
    doShowAll();
}
//-------------------------------------------------------------------------------------
//restart the local storage
function ClearAll() {
    localStorage.clear();
    doShowAll();
}
var cookieItems = [];
//--------------------------------------------------------------------------------------
// dynamically populate the table with shopping list items
//below step can be done via PHP and AJAX too. 
function doShowAll() {
    if (CheckBrowser()) {
        var key = "";
        var list = "<thead><tr><th>SL No.</th><th>Image</th><th>Quality</th><th>Product Name</th><th>Price</th><th>Remove</th></tr></thead><tbody>";
        var i = 0;
        var total = 0;
        //for more advance feature, you can set cap on max items in the cart
        for (i = 0; i <= localStorage.length - 1; i++) {
            debugger;
            key = localStorage.key(i);
            var obj = JSON.parse(localStorage.getItem(key));
            list += '<tr data-id="' + key + '" class="rem"><td class="invert">' + parseInt(i + 1) + '</td>' +
                                    '<td class="invert-image"><img src="../../images/'+obj["pic"]+'" alt=" " class="img-responsive" style="width: 50px; margin: 0 10px;"></td>' +
                                    '<td class="invert"><div class="quantity"><div class="quantity-select">' +
                                    '<div class="entry value-minus">&nbsp;</div><div class="entry value"><span>' + obj.quantity + '</span></div><div class="entry value-plus active">&nbsp;</div>' +
                                    '</div></div></td><td class="invert">' + obj.name + '</td><td class="invert">৳.' + obj.price + '</td>' +
                                    '<td class="invert"><div class="rem"><div class="deleteCart" > </div></div></td></tr>';
            
            total += parseInt(obj.quantity) * parseFloat(obj.price);

            cookieItems.push(obj);
        }
        list += '</tbody>';
        //if no item exists in the cart
        if (localStorage.length == 0) {
            list = "<tr><td colspan='5'>Your Cart Is Empty</td></tr>\n";
        }
        //bind the data to html table
        //you can use jQuery too....
        document.getElementById('cartItems').innerHTML = list;
        $('#cart_item').text(localStorage.length);
        $('#cart_amount').text(total);
        CartListFunction();
    } else {
        document.getElementById('cartItems').innerHTML = "";
        $('#cart_item').text('0');
        $('#cart_amount').text('0');
        alert('Cannot save shopping list as your browser does not support HTML 5');
    }
    console.log(cookieItems[0]);
}

/*
 =====> Checking the browser support
 //this step may not be required as most of modern browsers do support HTML5
 */
//below function may be redundant
function CheckBrowser() {
    if ('localStorage' in window && window['localStorage'] !== null) {
        // we can use localStorage object to store data
        return true;
    } else {
        return false;
    }
}
//-------------------------------------------------
/*
You can extend this script by inserting data to database or adding payment processing API to shopping cart..
*/