<script>
    export let date;
    let horaActual = new Date()
    $: tiempoRelativo = relativeTime(date, horaActual)
    
    setInterval(() => horaActual = new Date(), relativeTime(date, horaActual).diferencia < 60000?1000:6000)

    function relativeTime(date, horaActual) {
        if(horaActual == undefined) horaActual = new Date()
        if (typeof date === "string") date = Date.parse(date);

        let current = horaActual.getTime();
        let previous = date;
        const msPerMinute = 60 * 1000;
        const msPerHour = msPerMinute * 60;
        const msPerDay = msPerHour * 24;
        const msPerMonth = msPerDay * 30;
        const msPerYear = msPerDay * 365;

        let elapsed = current - previous;

        if (elapsed < msPerMinute) {
            return {
                short: Math.round(elapsed / 1000) + " s",
                long: Math.round(elapsed / 1000) + " segundos",
                diferencia: elapsed
            };
        } else if (elapsed < msPerHour) {
            return {
                short: Math.round(elapsed / msPerMinute) + " m",
                long: Math.round(elapsed / msPerMinute) + " minutos",
                diferencia: elapsed
            };
        } else if (elapsed < msPerDay) {
            return {
                short: Math.round(elapsed / msPerHour) + " h",
                long: Math.round(elapsed / msPerHour) + " horas",
                diferencia: elapsed
            };
        } else if (elapsed < msPerMonth) {
            return {
                short: Math.round(elapsed / msPerDay) + " d",
                long: Math.round(elapsed / msPerDay) + " dias",
                diferencia: elapsed
            };
        } else if (elapsed < msPerYear) {
            return {
                short: Math.round(elapsed / msPerMonth) + " M",
                long: Math.round(elapsed / msPerMonth) + " meses",
                diferencia: elapsed
            };
        } else {
            return {
                short: Math.round(elapsed / msPerYear) + " a",
                long: Math.round(elapsed / msPerYear) + " años",
                diferencia: elapsed
            };
        }
    }
</script>

<span title={tiempoRelativo.long + "\n" + new Date(date)}>{tiempoRelativo.short}</span>
