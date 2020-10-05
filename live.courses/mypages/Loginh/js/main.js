"use strict";

function validateForm(){
    var x = document.forms["form_login"]["username"].value;
    if( x="" ){
        document.getElementById("username").style.border = "red";
    }
}

validateForm();