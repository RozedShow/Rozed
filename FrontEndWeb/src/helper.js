function relativeTime(date) {

    let  current = new Date().getTime()
    let  previous = date.getTime()
    const  msPerMinute = 60 * 1000;
    const  msPerHour = msPerMinute * 60;
    const  msPerDay = msPerHour * 24;
    const  msPerMonth = msPerDay * 30;
    const  msPerYear = msPerDay * 365;

    let  elapsed = current - previous;

    if (elapsed < msPerMinute) {
            return Math.round(elapsed/1000) + ' s';   
    }

    else if (elapsed < msPerHour) {
            return Math.round(elapsed/msPerMinute) + ' m';   
    }

    else if (elapsed < msPerDay ) {
            return Math.round(elapsed/msPerHour ) + ' h';   
    }

    else if (elapsed < msPerMonth) {
        return  Math.round(elapsed/msPerDay) + ' d';   
    }

    else if (elapsed < msPerYear) {
        return  Math.round(elapsed/msPerMonth) + ' Mes';   
    }

    else {
        return  Math.round(elapsed/msPerYear ) + ' a';   
    }

}

export function formatearTiempo(tiempo) {
    return new Date(Date.parse(tiempo)).toLocaleString()
}

export function formatearTimeSpan(timespan)
{
    // 69444.10:39:00.0000010
    //"00:05:00.0000010",
    let minutos = Array.from(timespan.matchAll(/(\d\d):/g))[1][1]
    let horas = Array.from(timespan.matchAll(/(\d\d):/g))[0][1]
    let dias = timespan.split(".")[0]

    minutos = minutos[0] == "0" ?minutos[1] : minutos
    horas = horas[0] == "0" ?horas[1] : horas
    
    let ret = ""
    if(dias > 0) ret += `${dias}D `
    if(horas > 0) ret += `${horas}H `
    if(minutos > 0) ret += `${minutos} min `
    return ret
}