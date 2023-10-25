function MyCustomUploadAdapterPlugin(editor) {
    editor.plugins.get("FileRepository").createUploadAdapter = (loader) => {
        return new UploadAdapter(loader);
    };
}

ClassicEditor
    .create(document.querySelector(".pageContentEditor1"),
        {
            removePlugins: ["Title"],
            mediaEmbed: {
                previewsInData: true
            },
            fontSize: {
                options: [
                    11,
                    12,
                    13,
                    14,
                    15,
                    16,
                    17,
                    18,
                    19,
                    20,
                    21,
                    22,
                    23,
                    24,
                    25,
                    26,
                    27
                ]
            },
            extraPlugins: [MyCustomUploadAdapterPlugin],
            toolbar: {
                items: [
                    "heading",
                    "|",
                    "fontFamily",
                    "fontSize",
                    "fontBackgroundColor",
                    "fontColor",
                    "bold",
                    "italic",
                    "link",
                    "bulletedList",
                    "numberedList",
                    "|",
                    "indent",
                    "outdent",
                    "alignment",
                    "|",
                    "imageUpload",
                    "mediaEmbed",
                    "insertTable",
                    "blockQuote",
                    "specialCharacters",
                    "subscript",
                    "superscript",
                    "strikethrough",
                    "horizontalLine",
                    "exportWord",
                    "exportPdf",
                    "undo",
                    "redo"
                ]
            },
            image: {
                toolbar: [
                    "imageTextAlternative",
                    "imageStyle:full",
                    "imageStyle:side",
                    "linkImage"
                ]
            },
            table: {
                contentToolbar: [
                    "tableColumn",
                    "tableRow",
                    "mergeTableCells"
                ]
            },
            language: {
                // The UI will be English.
                ui: "ar",

                // But the content will be edited in Arabic.
                content: "ar"
            },
            licenseKey: "",

        })
    .then(editor => {
        window.pageContentEditor1 = editor;
        editor.setData($("#ArContent").val());
    })
    .catch(error => {
        console.error("Oops, something went wrong!");
        console.error(error);
    });
ClassicEditor
    .create(document.querySelector(".pageContentEditor2"),
        {
            removePlugins: ["Title"],
            mediaEmbed: {
                previewsInData: true
            },
            fontSize: {
                options: [
                    11,
                    12,
                    13,
                    14,
                    15,
                    16,
                    17,
                    18,
                    19,
                    20,
                    21,
                    22,
                    23,
                    24,
                    25,
                    26,
                    27
                ]
            },
            extraPlugins: [MyCustomUploadAdapterPlugin],
            toolbar: {
                items: [
                    "heading",
                    "|",
                    "fontFamily",
                    "fontSize",
                    "fontBackgroundColor",
                    "fontColor",
                    "bold",
                    "italic",
                    "link",
                    "bulletedList",
                    "numberedList",
                    "|",
                    "indent",
                    "outdent",
                    "alignment",
                    "|",
                    "imageUpload",
                    "mediaEmbed",
                    "insertTable",
                    "blockQuote",
                    "specialCharacters",
                    "subscript",
                    "superscript",
                    "strikethrough",
                    "horizontalLine",
                    "exportWord",
                    "exportPdf",
                    "undo",
                    "redo"
                ]
            },
            image: {
                toolbar: [
                    "imageTextAlternative",
                    "imageStyle:full",
                    "imageStyle:side",
                    "linkImage"
                ]
            },
            table: {
                contentToolbar: [
                    "tableColumn",
                    "tableRow",
                    "mergeTableCells"
                ]
            },
            language: {
                // The UI will be English.
                ui: "en",

                // But the content will be edited in Arabic.
                content: "en"
            },
            licenseKey: "",

        })
    .then(editor => {
        window.pageContentEditor2 = editor;
        editor.setData($("#EnContent").val());
    })
    .catch(error => {
        console.error("Oops, something went wrong!");
        console.error(error);
    });


document.querySelector("#addArticleForm").addEventListener("submit",
    (e) => {
        e.preventDefault();

        if (!$("#addArticleForm").valid()) {
            return;
        }

        $("#ArContent").val(window.pageContentEditor1.getData());
        $("#EnContent").val(window.pageContentEditor2.getData());


        var form = $("#addArticleForm");
        var url = form.attr("action");
        var form2 = $("form")[0]; // You need to use standard javascript object here
        var formData = new FormData(form2);

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
            processData: false, // NEEDED, DON'T OMIT THIS
            success: function(result) {
                Swal.fire("تمت العملية بنجاح", "", "success");
                setTimeout(() => {

                        window.location.href = `/Dashboard/Articles/${result}/Set`;
                    },
                    1000);
            },
            error: function() {
            }
        });
    });


//sort slider images 

$(".images-sortable").sortable();
let imagesOrders = [];
$(".images-sortable").on("sortupdate",
    function(e) {
        e.stopPropagation();
        imagesOrders = [];
        const imagesElme = e.currentTarget.querySelectorAll(".image-sortable");
        imagesElme.forEach((img, imgIndx) => {
            const imgSrc = img.querySelector("img").src;

            imagesOrders.push({
                Identifier: imgSrc,
                Order: imgIndx + 1

            });
        });

        const id = $("#Id").val();

        const url = `/Dashboard/Articles/${id}/RearrangeSliderImages`;
        $.ajax({
            url: url,
            type: "Post",
            data: JSON.stringify(imagesOrders),
            cache: false,
            processData: false,
            contentType: "application/json",
            success: function(result) {
                if (result) {
                    Swal.fire("تمت العملية بنجاح", "", "success");
                } else {
                    Swal.fire("حدث خطأ ما ", "", "Error");
                }
            },
            error: function(e) {

                console.log("ERROR : ", e);
                alert("Something seems Wrong while sending the Add request.");
            }
        });

    });