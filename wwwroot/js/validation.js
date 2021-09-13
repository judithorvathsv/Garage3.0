$.validator.addMethod('namecheck', function (value, element, params) {
    return document.querySelector("#FirstName").value !== document.querySelector("#LastName").value;
});

$.validator.unobtrusive.adapters.addBool("namecheck");