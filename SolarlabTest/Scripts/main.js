$(document).ready(function () {
    //отобразить панель добавления задачи и установить текущую дату в поле "дата выполнения"
    $("#show-add-task-form").click(function () {
        //установить текущую дату в поле "дата выполнения"
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = now.getFullYear() + "-" + (month) + "-" + (day);
        $('.execution-date-input').val(today);
        //отобразить панель добавления задачи
        $(".add-task-block").addClass("add-task-block-visible");
        $(".add-task-block").removeClass("add-task-block-hidden");
    });

    //закрыть панель добавления задачи
    $("#add-task-form-close-icon").click(function () {
        $(".add-task-block").addClass("add-task-block-hidden");
        $(".add-task-block").removeClass("add-task-block-visible");
    });

    //отобразить панель изменения задачи
    $(".change-task-link").click(function () {
        var taskId = $(this).attr("taskId");
        $.get("/Home/GetTaskInfo/" + taskId, function (taskData) {
            //заполнить поля текущими значениями
            $(".change-task-form .task-id-input").val(taskId);
            $(".change-task-form .task-name-input").val(taskData.TaskName);
            var date = eval(taskData.DeadlineDate.replace(/\/Date\((\d+)\)\//gi, 'new Date($1)'));
            var day = ("0" + date.getDate()).slice(-2);
            var month = ("0" + (date.getMonth() + 1)).slice(-2);
            var today = date.getFullYear() + "-" + (month) + "-" + (day);
            $(".change-task-form .execution-date-input").val(today);
            $(".change-task-form .execution-time-input").val(taskData.DeadlineTime);
            if (taskData.HighTaskPriority) {
                $('[value=True].hight-priority-input').prop("checked", true);
            }
            else {
                $('[value=False].hight-priority-input').prop("checked", true);
            }
        });
        //отобразить панель изменения задачи
        $(".change-task-block").addClass("change-task-block-visible");
        $(".change-task-block").removeClass("change-task-block-hidden");
    });

    //закрыть панель изменения задачи
    $("#change-task-form-close-icon").click(function () {
        $(".change-task-block").addClass("change-task-block-hidden");
        $(".change-task-block").removeClass("change-task-block-visible");
    });

});
