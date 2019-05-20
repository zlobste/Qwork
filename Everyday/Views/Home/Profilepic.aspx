﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title></title>
</head>
<body>



    <h2>MVC : Profile Image</h2>
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <link href="~/iEdit.css" rel="stylesheet" />
    <script src="~/iEdit.js"></script>
    <style type="text/css">
        #profilepic {
            display: block;
            position: relative;
            width: 20%;
        }
    </style>
    @using (Html.BeginForm("Profilepic", "login", FormMethod.Post, new { enctype = "multipart/form-data" }))
    {
        <table>
            <tr><td width="10%">Profile image :</td><td><input id="file" accept="image/*" type="file"><input type="hidden" name="imgbase64" id="imgbase64" /></td></tr>
            <tr><td></td><td><img id="profilepic" style="display:none"></td></tr>
            <tr><td></td><td><input type="submit" id="btnUpload" value="Save" /></td></tr>
        </table>
    }
    @Html.Raw(TempData["Success"])
    <script>
    $(document).ready(function () {
        $("#file").change(function (e) {
            var img = e.target.files[0];
            document.getElementById("profilepic").style.display = 'block';
            if (!iEdit.open(img, true, function (res) {
                $("#profilepic").attr("src", res);
               document.getElementById("imgbase64").value = res;
            })) {
                alert("Please check file type !!!");
            }
        });
    });
    </script>

</body>
</html>