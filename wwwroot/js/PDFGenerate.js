function DownloadPdf(filename, byteBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function base64Blob (b64Data) {
    sliceSize = 512;
    
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        
        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        
        var byteArray = new Uint8Array(byteNumbers);
        
        byteArrays.push(byteArray);
    }
    
    var blob = new Blob(byteArrays, { type: 'application/pdf' });
    return blob;
}