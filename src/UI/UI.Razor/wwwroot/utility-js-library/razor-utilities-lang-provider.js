var langDictionary = {
    deleteTitle: {
        ar: "تأكيد العملية",
        en: "Please Confirm"
    },
    deleteContent: {
        ar: "هل أنت متأكد من إتمام عملية الحذف؟",
        en: "Are you sure you want to delete?"
    },
    deleteYes: {
        ar: "نعم, إحذف!",
        en: "Yes, Delete!"
    },
    deleteNo: {
        ar: "لا, ألغ العملية",
        en: "No, Cancel"
    }
};

function getValue(key) {
    var lang = document.documentElement.getAttribute("dir");
    if (lang == "rtl")
        return langDictionary[key].ar;
    return langDictionary[key].en;
}