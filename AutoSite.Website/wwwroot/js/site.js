
(function ($) {
    $(document).on("click", ".uploader", function () {
        $(".auto-upload", $(this).closest("form")).click();
    })
        .on("change", ".auto-upload", function () {
            if($($(this).closest("form")).valid())
                $(this).closest("form").submit(); 
    });
} (jQuery));