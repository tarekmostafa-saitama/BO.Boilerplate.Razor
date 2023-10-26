$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

// Function for confirming and deleting an element.
function DeleteConfirmation(
    url,
    reload = true,
    method = "GET",
    removedElementId = null,
    title = "Are you sure ?",
    text = "Are you sure you want to perform this operation ?",
    icon = "warning"
) {
    swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "نعم",
        cancelButtonText: "لا",
    }).then(function(choice) {
        if (choice.value) {
            $.ajax({
                url: url,
                type: method,
                success: function(result) {
                    if (result.success) {
                        Swal.fire(result.message);
                        if (reload) location.reload(true);
                        if (removedElementId) {
                            var element = document.getElementById(removedElementId);
                            if (element) element.remove();
                        }
                    } else {
                        Swal.fire(result.message);
                    }
                },
                error: function(xhr) {
                    console.error("Error: ", xhr); // Marked the change with 'console.error'
                    alert("Something seems Wrong");
                },
            });
        }
    });
}

// Function for confirming and deleting a sortable element.
function DeleteSortableConfirmation(
    url,
    reload = true,
    method = "GET",
    removedElementId = null,
    title = "Are you sure ?",
    text = "Are you sure you want to perform this operation ?",
    icon = "warning"
) {
    swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "نعم",
        cancelButtonText: "لا",
    }).then(function(choice) {
        if (choice.value) {
            $.ajax({
                url: url,
                type: method,
                success: function(result) {
                    if (result.success) {
                        Swal.fire(result.message);

                        // Enhanced the following lines:
                        if (removedElementId) {
                            var element = document.getElementById(removedElementId);
                            if (element) element.remove();

                            var elements = document.getElementsByClassName(removedElementId);
                            if (elements.length > 0) elements[0].remove();
                        }
                    } else {
                        Swal.fire(result.message);
                    }
                },
                error: function(xhr) {
                    console.error("Error: ", xhr); // Marked the change with 'console.error'
                    alert("Something seems Wrong");
                },
            });
        }
    });
}

// Function for confirming and deleting an element from a data table.
function DeleteDataTableConfirmation(
    url,
    reload = true,
    method = "GET",
    removedElementId = null,
    title = "Are you sure ?",
    text = "Are you sure you want to perform this operation ?",
    icon = "warning"
) {
    swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "نعم",
        cancelButtonText: "لا",
    }).then(function(choice) {
        if (choice.value) {
            $.ajax({
                url: url,
                type: method,
                success: function(result) {
                    if (result.success) {
                        Swal.fire(result.message);
                        if (reload) location.reload(true);
                        if (removedElementId) {
                            var dtRow = $("#" + removedElementId).closest("tr");
                            if (dtRow) {
                                var dataTable = window.dataTable;
                                dataTable.row(dtRow).remove().draw(false);
                            }
                        }
                    } else {
                        Swal.fire(result.message);
                    }
                },
                error: function(xhr) {
                    console.error("Error: ", xhr); // Marked the change with 'console.error'
                    alert("Something seems Wrong");
                },
            });
        }
    });
}


function ConfigureSortable(sortingContainerElm, sortActionUrl) {
    $(function() {
        $(sortingContainerElm).sortable();
        $(sortingContainerElm).on("sortupdate",
            function(e) {
                var sortedElmsOrders = [];
                var sortedElms = Array.from(this.children);
                sortedElms.forEach((elm, i) => {
                    var elementId = elm.querySelector(".sortableElementId").value;
                    sortedElmsOrders.push({
                        Id: elementId,
                        Order: i + 1,
                    });
                });
                // send post request with new orders after sorting
                $.ajax({
                    url: sortActionUrl,
                    type: "Post",
                    data: JSON.stringify(sortedElmsOrders),
                    contentType: "application/json",
                    dataType: "json",
                    success: function(result) {
                        if (result.success) {
                            Toastify({
                                text: result.message,
                                duration: 3000,
                                newWindow: true,
                                close: true,
                                gravity: "bottom", // `top` or `bottom`
                                position: "left", // `left`, `center` or `right`
                                stopOnFocus: false, // Prevents dismissing of toast on hover
                                style: {
                                    background: "#1ABC9C",
                                    width: "250px",
                                    "font-weight": "bolder",
                                },
                                onClick: function() {} // Callback after click
                            }).showToast();
                        } else {
                            Toastify({
                                text: result.message,
                                duration: 3000,
                                newWindow: true,
                                close: true,
                                gravity: "bottom", // `top` or `bottom`
                                position: "left", // `left`, `center` or `right`
                                stopOnFocus: false, // Prevents dismissing of toast on hover
                                style: {
                                    background: "#E7515A",
                                    width: "250px",
                                    "font-weight": "bolder",
                                },
                                onClick: function() {} // Callback after click
                            }).showToast();
                        }
                    },
                    error: function(e) {
                        console.error("Error: ", e); // Marked the change with 'console.error'
                        alert("Something seems Wrong while sending the Add request.");
                    },
                });
            });
    });
}


var setInnerHTML = function(elm, html) {
    elm.innerHTML = html;
    Array.from(elm.querySelectorAll("script")).forEach((oldScript) => {
        var newScript = document.createElement("script");
        Array.from(oldScript.attributes).forEach((attr) =>
            newScript.setAttribute(attr.name, attr.value)
        );
        newScript.appendChild(document.createTextNode(oldScript.innerHTML));
        oldScript.parentNode.replaceChild(newScript, oldScript);
    });
};

function getPartialViewModal(url,
    title,
    modalId = "generalModal",
    modalTitleId = "generalModalTitle",
    modalBodyId = "generalModalBody",
    method = "GET") {
    $.ajax({
        url: url,
        type: method,
        cache: false,
        processData: false,
        contentType: false,
        success: function(partialViewContent) {
            document.getElementById(modalTitleId).textContent = title;
            setInnerHTML(
                document.getElementById(modalBodyId),
                partialViewContent
            );
            $("#" + modalId).modal("show");
            if ($(partialViewContent).find("form").length > 0)
                $.validator.unobtrusive.parse($("#" + modalId + "form"));

        },
        error: function(e) {
            console.error("ERROR : ", e);
            alert("Something seems Wrong while sending the sending request.");
        },
    });
}


function confirmAjaxOperation(
    url,
    reload = true,
    method = "GET",
    title = "Are you sure ?",
    text = "Are you sure you want to perform this operation ?",
    icon = "warning"
) {
    swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "نعم",
        cancelButtonText: "لا",
    }).then(function(choice) {
        if (choice.value) {
            $.ajax({
                url: url,
                type: method,
                success: function(result) {
                    if (result.success) {
                        Swal.fire(result.message);
                        if (reload)
                            setTimeout(() => {
                                    location.reload(true);
                                },
                                2000);
                    } else {
                        Swal.fire(result.message);
                    }
                },
                error: function(xhr) {
                    console.error("Error: ", xhr); // Marked the change with 'console.error'
                    alert("Something seems Wrong");
                },
            });
        }
    });
}

function ChangeIframesSize(selector, width, height) {
    document.querySelectorAll(selector).forEach((iframe) => {
        iframe.style.width = width;
        iframe.style.height = height;
    });
}



    $(document).on("submit", "form",
        (event) => {
            if (!event.currentTarget.classList.contains("formDeleteConfirmation"))
                event.currentTarget.submit();
            event.preventDefault();
            swal.fire({
                title: getValue("deleteTitle"),
                text: getValue("deleteContent"),
                icon: "info",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: getValue("deleteYes"),
                cancelButtonText: getValue("deleteNo")
            }).then(function(choice) {
                if (choice.value) {
                    event.currentTarget.submit();
                }
            });
        });