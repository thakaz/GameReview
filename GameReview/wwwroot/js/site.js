// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var fileArea = document.getElementById('dragDropArea');
var fileInput = document.getElementById('ImageFile');
fileArea.addEventListener('dragover', function (evt) {
    evt.preventDefault();
    fileArea.classList.add('dragover');
});
fileArea.addEventListener('dragleave', function (evt) {
    evt.preventDefault();
    fileArea.classList.remove('dragover');
});
fileArea.addEventListener('drop', function (evt) {
    evt.preventDefault();
    fileArea.classList.remove('dragenter');
    var files = evt.dataTransfer.files;
    console.log("DRAG & DROP");
    console.table(files);
    fileInput.files = files;
    imgPreview('onChenge', files[0]);
});


function imgPreview(event, f = null) {
    var file = f;
    if (file === null) {
        file = event.target.files[0];
    }
    var reader = new FileReader();
    var preview = document.getElementById("previewArea");
    var previewImage = document.getElementById("previewImage");

    if (previewImage != null) {
        preview.removeChild(previewImage);
    }
    reader.onload = function (event) {
        var img = document.createElement("img");
        img.setAttribute("src", reader.result);
        img.setAttribute("id", "previewImage");
        img.classList.add("img-thumbnail");
        preview.appendChild(img);
    };

    reader.readAsDataURL(file);

    changeBGImage();
}

function changeBGImage(imgPath) {
    document.querySelector('#detailContainer').style.backgroundImage ='url("' + imgPath + '")'
}

function sortFormSubmit(value) {
    document.sortSelectForm.submit();
}