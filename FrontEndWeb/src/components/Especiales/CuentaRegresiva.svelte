<script>
    import {cuentaRegresivaStore} from './CuentaRegresivaStore'

    let dias = 0
    let horas = 0 
    let minutos = 0
    let segundos = 0

    // let futureDate = new Date(new Date().getTime() + 5000)
    // let fechaFutura = new Date("Dec 31 2020 19:13:00")
    // let fechaActual = new Date();
    let reproduciendoSoleado = false

    // let soleado  = new Audio("/audio/soleado.mp3")
    let countDown = () => {
        $cuentaRegresivaStore.fechaActual = new Date();
        let myDate = $cuentaRegresivaStore.fechaFutura - $cuentaRegresivaStore.fechaActual;
        
        dias = Math.floor(myDate / 1000 / 60 / 60 / 24);
        horas = Math.floor(myDate / 1000 / 60 / 60 ) % 24;
        minutos = Math.floor(myDate / 1000 / 60  ) % 60;
        segundos = Math.floor(myDate / 1000 ) % 60;

        // if($cuentaRegresivaStore.fechaFutura < $cuentaRegresivaStore.fechaActual && !reproduciendoSoleado) {
        //     soleado.currentTime = ($cuentaRegresivaStore.fechaActual.getTime() - $cuentaRegresivaStore.fechaFutura.getTime()) / 1000
        //     try {
        //         soleado.play()
        //     } catch(e) {
        //         console.log(e);
        //     }
        //     reproduciendoSoleado = true;
            
        // }
    }




    countDown()

    setInterval(countDown, 1000)
</script>
<div class="cuenta-regresiva">
    {#if $cuentaRegresivaStore.fechaFutura > $cuentaRegresivaStore.fechaActual}
        <span>Se viene en</span>
        <div class="countdown-container">
            <span id="hours" class="big-text">{horas}</span>
            <span>Hora{horas != 1 ?'s':''}</span>
            <span id="min" class="big-text">{minutos}</span>
            <span>Minuto{minutos != 1 ?'s':''}</span>
            <span id="sec" class="big-text">{segundos}</span>
            <span>Segundo{segundos != 1 ?'s':''}</span>
        </div>
    {:else}
        <div>Feliz año nuevo!</div>
        <div>2021</div>
    {/if}
</div>

<style>
    .cuenta-regresiva {
        font-family: 'euroFighter';
        font-size: 20px;
        color: #cc1d1d;
        text-align: center;
        position: absolute;
        padding-top: 4px;
        width: 100%;
        pointer-events: none !important;
    }

    @media (max-width: 600px) {
        .cuenta-regresiva {
            font-size: 4px !important;
            height: 100%;
        }
    }
    :global(.modoSticky) .cuenta-regresiva {
        font-size: 13px;
    }

</style>

