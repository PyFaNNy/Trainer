<script>
    var canvas = document.getElementById("myChart");
    var context = canvas.getContext("2d");

    var lines = 200,
    frag = canvas.clientWidth / lines,
    scale = canvas.clientHeight / 2;

    context.moveTo(0, scale);
    for (var i = 0; i < lines; i++) {
        var sine = Math.sin(i / scale * 2) * scale;

    context.lineTo(i * frag, -sine + scale);
    }
    context.stroke();

    canvas = document.getElementById("myChart1");
    context = canvas.getContext("2d");

    frag = canvas.clientWidth / lines,
    scale = canvas.clientHeight / 2;

    context.moveTo(0, scale);
    for (var i = 0; i < lines; i++) {
        var sine = Math.sin(i / scale * 2) * scale;
        context.lineTo(i * frag, -sine + scale);
    }
    context.stroke();

    canvas = document.getElementById("myChart2");
    context = canvas.getContext("2d");

    lines = 200,
    frag = canvas.clientWidth / lines,
    scale = canvas.clientHeight / 2;

    context.moveTo(0, scale);
    for (var i = 0; i < lines; i++) {
        var sine = Math.sin(i / scale * 2) * scale;

    context.lineTo(i * frag, -sine + scale);
    }
    context.stroke();

    canvas = document.getElementById("myChart3");
    context = canvas.getContext("2d");

    lines = 200,
    frag = canvas.clientWidth / lines,
    scale = canvas.clientHeight / 2;

    context.moveTo(0, scale);
    for (var i = 0; i < lines; i++) {
        var sine = Math.sin(i / scale * 2) * scale;

    context.lineTo(i * frag, -sine + scale);
    }
    context.stroke();
</script>