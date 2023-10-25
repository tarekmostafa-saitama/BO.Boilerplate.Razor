var myModalEl,
    kanbanboard,
    scroll,
    addNewBoard,
    addMember,
    profileField,
    reader,
    tasks_list = [
        document.getElementById("pending-tasks"),
        document.getElementById("todo-tasks"),
        document.getElementById("inprogress-tasks"),
        document.getElementById("hold-tasks"),
        document.getElementById("review-tasks"),
        document.getElementById("cancelled-tasks"),
    ];

function noTaskImage() {
    Array.from(document.querySelectorAll("#kanbanboard .tasks-list")).forEach(function(e) {
        0 < e.querySelectorAll(".tasks-box").length
            ? e.querySelector(".tasks").classList.remove("noTask")
            : e.querySelector(".tasks").classList.add("noTask");
    });
}

function taskCounter() {
    (task_lists = document.querySelectorAll("#kanbanboard .tasks-list")) &&
        Array.from(task_lists).forEach(function(e) {
            (tasks = e.getElementsByClassName("tasks")),
                Array.from(tasks).forEach(function(e) {
                    (task_box = e.getElementsByClassName("tasks-box")), (task_counted = task_box.length);
                }),
                (badge = e.querySelector(".totaltask-badge").innerText = ""),
                (badge = e.querySelector(".totaltask-badge").innerText = task_counted);
        });
}

function newKanbanbaord() {
    var e = document.getElementById("boardName").value,
        a = Math.floor(100 * Math.random()),
        t = "review_task_" + a;
    (kanbanlisthtml =
            '<div class="tasks-list" id=' +
            ("remove_item_" + a) +
            '><div class="d-flex mb-3"><div class="flex-grow-1"><h6 class="fs-14 text-uppercase fw-semibold mb-0">' +
            e +
            '<small class="badge bg-success align-bottom ms-1 totaltask-badge">0</small></h6></div><div class="flex-shrink-0"><div class="dropdown card-header-dropdown"><a class="text-reset dropdown-btn" href="#" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fw-medium text-muted fs-12">Priority<i class="mdi mdi-chevron-down ms-1"></i></span></a><div class="dropdown-menu dropdown-menu-end"><a class="dropdown-item" href="#">Priority</a><a class="dropdown-item" href="#">Date Added</a></div></div></div></div><div data-simplebar class="tasks-wrapper px-3 mx-n3"><div class="tasks" id="' +
            t +
            '" ></div></div><div class="my-3"><button class="btn btn-soft-info w-100" data-bs-toggle="modal" data-bs-target="#creatertaskModal">Add More</button></div></div>'),
        document.getElementById("kanbanboard").insertAdjacentHTML("beforeend", kanbanlisthtml),
        document.getElementById("addBoardBtn-close").click(),
        noTaskImage(),
        taskCounter(),
        drake.destroy(),
        tasks_list.push(document.getElementById(t)),
        (drake = dragula(tasks_list).on("out",
            function(e, a) {
                noTaskImage(), taskCounter();
            })),
        (document.getElementById("boardName").value = "");
}

function newMemberAdd() {
    var e = document.getElementById("firstnameInput").value,
        a = localStorage.getItem("kanbanboard-member");
    (newMembar =
            '<a href="javascript: void(0);" class="avatar-group-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="' +
            e +
            '">' +
            a +
            "</a>"),
        document.getElementById("newMembar").insertAdjacentHTML("afterbegin", newMembar),
        document.getElementById("btn-close-member").click();
}

tasks_list &&
    ((myModalEl = document.getElementById("deleteRecordModal")) &&
            myModalEl.addEventListener("show.bs.modal",
                function(e) {
                    document.getElementById("delete-record").addEventListener("click",
                        function() {
                            e.relatedTarget.closest(".tasks-box").remove(), document.getElementById("delete-btn-close")
                                .click(), taskCounter();
                        });
                }),
        (drake = dragula(tasks_list,
                {
                    accepts: (el, target, source, sibling) => {
                        var teamId = el.querySelector(".teamId").value;
                        if (source.id == "pending-tasks") {
                            if (!teamId) {
                                showToast("Please assign team before moving to another status");
                                return false;
                            }

                        }
                        var assignes = el.querySelector(".assignes").value;

                        if (source.id == "todo-tasks") {
                            if (assignes == 0 && target.id != "pending-tasks" && target.id != source.id) {
                                showToast(
                                    "There is no assigned members for this task so you cannot moving it to another status");
                                return false;

                            }
                        }

                        return true;
                    }
                })
            .on("drag",
                function(e) {
                    e.className = e.className.replace("ex-moved", "");

                })
            .on("drop",
                function(e, target, source) {

                    var taskId = e.querySelector(".taskId").value;

                    if (target.id == "pending-tasks") {
                        changeStatus(taskId, 0);
                    } else if (target.id == "todo-tasks") {
                        changeStatus(taskId, 1);


                    } else if (target.id == "inprogress-tasks") {
                        changeStatus(taskId, 2);


                    } else if (target.id == "hold-tasks") {
                        changeStatus(taskId, 3);

                    } else if (target.id == "review-tasks") {
                        changeStatus(taskId, 4);

                    } else {
                        changeStatus(taskId, 5);
                    }

                    e.className += " ex-moved";


                })
            .on("over",
                function(e, a) {
                    a.className += " ex-over";
                })
            .on("out",
                function(e, a) {
                    (a.className = a.className.replace("ex-over", "")), noTaskImage(), taskCounter();
                })),
        (kanbanboard = document.querySelectorAll("#kanbanboard")) &&
        (scroll = autoScroll([document.querySelector("#kanbanboard")],
            {
                margin: 20,
                maxSpeed: 100,
                scrollWhenOutside: !0,
                autoScroll: function() {
                    return this.down && drake.dragging;
                },
            })),
        (addNewBoard = document.getElementById("addNewBoard")) &&
            document.getElementById("addNewBoard").addEventListener("click", newKanbanbaord),
        (addMember = document.getElementById("addMember"))) &&
    (document.getElementById("addMember").addEventListener("click", newMemberAdd),
        (profileField = document.getElementById("profileimgInput")),
        (reader = new FileReader()),
        profileField.addEventListener("change",
            function(e) {
                reader.readAsDataURL(profileField.files[0]),
                (reader.onload = function() {
                    var e = reader.result;
                    localStorage.setItem("kanbanboard-member",
                        '<img src="' + e + '" alt="profile" class="rounded-circle avatar-xs">');
                });
            }));

var notificationTimer = null;

function showToast(message) {
    if (notificationTimer) {
        clearTimeout(notificationTimer);
    }
    notificationTimer = setTimeout(() => {
            // Show a single notification about items not being draggable
            Toastify({
                text: message,
                duration: 5000,
                newWindow: true,
                close: true,
                gravity: "bottom", // `top` or `bottom`
                position: "right", // `left`, `center` or `right`
                stopOnFocus: false, // Prevents dismissing of toast on hover
                style: {
                    background: "#d29c40",
                    width: "250px",
                    'font-weight': "bolder",

                },
                onClick: function() {} // Callback after click
            }).showToast();
        },
        500);
}


function changeStatus(taskId, status) {


    $.ajax({
        type: "POST",
        url: `/Traffic/${taskId}/ChangeTaskStatus`,
        data: { taskId: taskId, status: status },
        success: function(data) {
            if (data.success) {

                Toastify({
                    text: "Data Saved Successfully",
                    duration: 5000,
                    newWindow: true,
                    close: true,
                    gravity: "bottom", // `top` or `bottom`
                    position: "right", // `left`, `center` or `right`
                    stopOnFocus: false, // Prevents dismissing of toast on hover
                    style: {
                        background: "#1abc9c",
                        width: "250px",
                        'font-weight': "bolder",

                    },
                    onClick: function() {} // Callback after click
                }).showToast();

                setTimeout(function() {
                        location.reload();
                    },
                    1000);
            } else {
                Toastify({
                    text: data.errors[0],
                    duration: 5000,
                    newWindow: true,
                    close: true,
                    gravity: "bottom", // `top` or `bottom`
                    position: "right", // `left`, `center` or `right`
                    stopOnFocus: false, // Prevents dismissing of toast on hover
                    style: {
                        background: "#d29c40",
                        width: "250px",
                        'font-weight': "bolder",

                    },
                    onClick: function() {} // Callback after click
                }).showToast();
            }
        },
        error: function(data) {
            Toastify({
                text: "an error has raise!!",
                duration: 5000,
                newWindow: true,
                close: true,
                gravity: "bottom", // `top` or `bottom`
                position: "right", // `left`, `center` or `right`
                stopOnFocus: false, // Prevents dismissing of toast on hover
                style: {
                    background: "#d29c40",
                    width: "250px",
                    'font-weight': "bolder",

                },
                onClick: function() {} // Callback after click
            }).showToast();


        }

    });

}