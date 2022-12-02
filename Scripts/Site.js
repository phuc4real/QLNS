function submitResult(e, id, text) {
    e.preventDefault();
    Swal.fire({
        title: 'Xác nhận',
        text: "Xóa "+text+" này?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xác nhận!',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Đã xóa!',
                 text+' đã được xoá.',
                'success'
            ).then((result) => document.getElementById("Delete" + id).submit())
        }
    })
}

//Sua thong tin sach
function enableEditCB() {
    $("#UP_ANH_BIA").removeAttr('hidden');
    $("#Edit_Button").removeAttr('hidden');
    $('#enable_Edit').attr('hidden', true);
    $('#disable_Edit').removeAttr('hidden');
    $("#TEN_SACH").removeAttr('readonly');
    $("#MA_DS").removeAttr('Disabled');
    $("#MA_TL").removeAttr('Disabled');
    $("#MA_DT").removeAttr('Disabled');
    $("#SO_TIEN").removeAttr('readonly');
    $("#TAC_GIA").removeAttr('readonly');
    $("#NHA_XB").removeAttr('readonly');
    $("#NAM_XB").removeAttr('readonly');
    $("#SL_TON").removeAttr('readonly');
    $("#MO_TA_SACH").removeAttr('readonly');

}
function disableEditCB() {
    $("#UP_ANH_BIA").attr('hidden', true);
    $("#Edit_Button").attr('hidden', true);
    $('#enable_Edit').removeAttr('hidden');
    $('#disable_Edit').attr('hidden', true);
    $("#TEN_SACH").attr('readonly', true);
    $("#MA_DS").prop("disabled", true);
    $("#MA_TL").prop("disabled", true);
    $("#MA_DT").prop("disabled", true);
    $("#SO_TIEN").attr('readonly', true);
    $("#TAC_GIA").attr('readonly', true);
    $("#NHA_XB").attr('readonly', true);
    $("#NAM_XB").attr('readonly', true);
    $("#SL_TON").attr('readonly', true);
    $("#MO_TA_SACH").attr('readonly', true);
}

//Trang ban hang
function AddToCart(MA_SACH) {
    $.ajax({
        type: 'POST', url: '/Sale/AddToCart', data: { id: MA_SACH }, success: function (data) {
            $("#Cart_Amount").html(data.ItemAmount);
        }
    });
}

//Gio hàng
const money = new Intl.NumberFormat('de-DE',
    { style: 'currency', currency: 'VND' });
function UpdateCart(ProductId) {
    var i = 0;
    if ("#Quality[txt-id='" + ProductId + "']" !== '') {
        i = $("#Quality[txt-id='" + ProductId + "']").val();
    }
    $.ajax({
        type: 'POST',
        url: '/Sale/UpdateCart',
        data: { id: ProductId, quality: i },
        success: function (data) {
            $("#Quality[txt-id='" + ProductId + "']").val(data.ItemAmount);
            $("#Price[txt-id='" + ProductId + "']").text(money.format(data.SumPrice));
            $("#Error[txt-id='" + ProductId + "']").text(data.Error);
            $("#Total").text("Tổng tiền: " + money.format(data.Total));
            $("#Cart_Amount").html(data.cartcount);
        }
    });
}

function payResult(e) {
    e.preventDefault();
    Swal.fire({
        title: 'Xác nhận',
        text: "Xác nhận thanh toán sản phẩm?",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xác nhận!',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Đã đặt hàng!',
                'Sản phẩm đã được thanh toán!.',
                'success'
            ).then((result) => document.getElementById("Payment").submit())
        }
    })
}

//Thong tin nhan vien
//Sua thong tin sach
function enableEditSI() {
    $("#Edit_ButtonSI").removeAttr('hidden');
    $('#enable_EditSI').attr('hidden', true);
    $('#disable_EditSI').removeAttr('hidden');
    $("#TEN_NV").removeAttr('readonly');
    $("#NAM_SINH").removeAttr('readonly');
    $("#NOI_SONG").removeAttr('readonly');
    $("#SDT").removeAttr('readonly');
}
function disableEditSI() {
    $("#Edit_ButtonSI").attr('hidden', true);
    $('#enable_EditSI').removeAttr('hidden');
    $('#disable_EditSI').attr('hidden', true);
    $("#TEN_NV").attr('readonly', true);
    $("#NAM_SINH").attr('readonly', true);
    $("#NOI_SONG").attr('readonly', true);
    $("#SDT").attr('readonly', true);
}