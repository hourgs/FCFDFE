
$(function () {
    $(".text-star").append('<span class="text-red">*</span>');
    $(".text-password").prop('type', 'password');

    $('.text-toUpper').on('keyup', function (event) {
        var strVal = $(this).val();
        strVal = strVal.toUpperCase(); //轉大寫
        $(this).val(strVal);
    });
    $(".text-toUpper").change(function () {
        var strVal = $(this).val();
        strVal = strVal.toUpperCase(); //轉大寫
        $(this).val(strVal);
    });
});

function txtKeyNumber() {
    //判斷如果輸入的不是數字或小數點!那將無法輸入文字
    if (((event.keyCode >= 48) && (event.keyCode <= 57)) || (event.keyCode == 46))
    { event.returnValue = true; }
    else
    { event.returnValue = false; }
}