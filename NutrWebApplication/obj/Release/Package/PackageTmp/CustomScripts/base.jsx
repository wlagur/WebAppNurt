function ajaxCall(url, data, onSuccess) {
    $.ajax({
        method: "POST",
        url: url,
        data: data,
        dataType: 'json',
        success: function (data) {
            onSuccess(data);
        }
    })
      .done(function (msg) {
          console.log("done");
      });
}