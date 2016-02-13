var taskid;
var taskname;

$(document.body).on('click', '.open-modal' ,function(e){
    taskid = this.dataset.taskid;
    $('#error').addClass('hidden');
    var t = document.getElementById('lb-' + taskid);
    if (this.dataset.type == 'change') {
        document.getElementById('tb-task-name').value = t.innerText;
    }
    else if (this.dataset.type == 'add') {
        taskname = t.innerText;
    }
    else if (this.dataset.type == 'new') {
        taskid = 'id';
    }

});

$('.task-btn-change-name').on('click', function (e) {
    var task = document.getElementById('lb-' + taskid);
    var newtaskname = document.getElementById('tb-task-name').value;
    
    var action = TaskAction('change', newtaskname, [taskid], 0, 0);

    if (action) {
        task.innerHTML = newtaskname;
    }
    else {
        $('#error').removeClass('hidden');
    }
    document.getElementById('tb-task-name').value = '';

});

$('.task-btn-new-task').on('click', function (e) {
    if (taskid != 'id') {
        var newtaskname = document.getElementById('tb-subtask-name').value;
        var action = TaskAction('new', newtaskname, null, taskid, 0);
        if (action) {
            newtaskid = TaskAction('get', newtaskname, null, 0, 0);
            var newtask = $("<li>").attr('id', 'task-li-' + newtaskid).addClass('bg-task-child');
            var newtasktext = document.getElementById('task-li-' + taskid);
            newtasktext = newtasktext.innerHTML;
            var re1 = new RegExp('-' + taskid, 'g');
            var re2 = new RegExp('"' + taskid + '"', 'g');
            newtasktext = newtasktext.replace(re1, '-' + newtaskid).replace(re2, '"' + newtaskid + '"');
            newtask.html(newtasktext);
            var innerlist = newtask.find('.task-list')[0];
            innerlist.innerHTML = '';
            var childlist = $("#task-children-" + taskid);
            childlist.append(newtask);
            document.getElementById('lb-' + newtaskid).innerHTML = newtaskname;
        }
        else {
            $('#error').removeClass('hidden');
        }

        document.getElementById('tb-subtask-name').value = '';
    }
    else {
        var newtaskname = document.getElementById('tb-subtask-name').value;

        var action = TaskAction('new', newtaskname, null, 0, 0);
        if (action) {
            newtaskid = TaskAction('get', newtaskname, null, 0, 0);
            var newtask = $("<li>").attr('id', 'task-li-' + newtaskid).addClass('bg-task-parent');
            var newtasktext = document.getElementById('task-li-' + taskid);
            newtasktext = newtasktext.innerHTML;
            var re1 = new RegExp('-' + taskid, 'g');
            var re2 = new RegExp(':' + taskid + ':', 'g');
            newtasktext = newtasktext.replace(re1, '-' + newtaskid).replace(re2, newtaskid);
            newtask.html(newtasktext);
            var childlist = $("#parent-list");
            childlist.append(newtask);
            document.getElementById('lb-' + newtaskid).innerHTML = newtaskname;
        }
        else {
            $('#error').removeClass('hidden');
        }
        document.getElementById('tb-subtask-name').value = '';
    }
});


$('.task-btn-remove-task').on('click', function (e) {
    var removeids = [taskid];
    var task = document.getElementById('task-li-' + taskid);
    var childtasks = task.getElementsByClassName('bg-task-child');

    for (var i = 0; i < childtasks.length; i++) {
        var item = childtasks[i];
        var childtaskid = item.id.split('-')[2];
        removeids.push(childtaskid);
    }
    var action = TaskAction('delete', null, removeids, 0, 0);
    if(action){
        task.parentNode.removeChild(task);
    }
    else {
        $('#error').removeClass('hidden');
    }
});

$('.task-box').on('change', function (e) {
    var taskid = this.dataset.taskid;
    var status = this.checked ? 100 : 0;
    var checkids = [taskid];

    var task = document.getElementById('task-li-' + taskid);
    var childtasks = task.getElementsByClassName('bg-task-child');

    for (var i = 0; i < childtasks.length; i++) {
        var item = childtasks[i];
        var childtaskid = item.id.split('-')[2];
        var childcb = document.getElementById('cb-' + childtaskid);
        childcb.checked = this.checked;
        checkids.push(childtaskid);
    }
    
    TaskAction('status', null, checkids, 0, status);
});

$(document.body).on('click', '.task-minmax' ,function(e){
    var taskid = this.dataset.taskid;
    var children = $('#task-children-' + taskid);
    children.toggleClass('hidden');
    if(this.innerText == '-') {
        this.innerText = '+';
    }
    else {
        this.innerText = '-';
    }
    
});


function TaskAction(type, text,  taskids, parent, status) {
    var ajaxresponse;
    $.ajax({
        type: "POST",
        url: '/Tracker/TaskAction',
        dataType: "json",
        async: false,
        data: { type: type, text: text, taskids: taskids, parent:parent, status:status },
        success: function (response) {
            ajaxresponse = response;
        },
        error: function () {
            ajaxresponse = false;
        }
    });
    return ajaxresponse;
}