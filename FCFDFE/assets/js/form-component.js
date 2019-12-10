var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_pageLoaded(newdatetime);

var Script = function () {
    
    if (top.location !== location) {
        top.location.href = document.location.href ;
    }
    $(function(){
        window.prettyPrint && prettyPrint();
        $(".timepicker").datetimepicker({
            pickDate: false,
            format: 'hh:mm',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
        $(".datetimepicker").datetimepicker({
            format: 'yyyy-MM-dd hh:mm:ss',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
        $(".datepicker").datetimepicker({
            pickTime: false,
            format: 'yyyy-MM-dd',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
        $(".datepicker-left").datetimepicker({
            pickTime: false,
            format: 'yyyy-MM-dd',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
    });

}();


function newdatetime() {

    if (top.location !== location) {
        top.location.href = document.location.href;
    }
    $(function () {
        window.prettyPrint && prettyPrint();
        $(".timepicker").datetimepicker({
            pickDate: false,
            format: 'hh:mm',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
        $(".datetimepicker").datetimepicker({
            format: 'yyyy-MM-dd hh:mm:ss',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
        $(".datepicker").datetimepicker({
            pickTime: false,
            format: 'yyyy-MM-dd',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
        $(".datepicker-left").datetimepicker({
            pickTime: false,
            format: 'yyyy-MM-dd',
            autoclose: 1,
            weekStart: 1,
            startView: 2,
            todayBtn: 1,
            todayHighlight: 1,
            forceParse: 0,
            minView: 2,
            pickerPosition: 'right'
        });
    });

}
