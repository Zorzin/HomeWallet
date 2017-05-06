$('#statisticstab').click(function () {
    $(this).parent().addClass('active');
    $('#detailstab').parent().removeClass('active');
    $('#detailsdiv').hide();
    $('#statisticsdiv').show();
});
$('#detailstab').click(function () {
    $(this).parent().addClass('active');
    $('#statisticstab').parent().removeClass('active');
    $('#detailsdiv').show();
    $('#statisticsdiv').hide();
});