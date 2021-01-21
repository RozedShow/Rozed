<script>
    import { Ripple } from "svelte-mui";
    import { fly } from "svelte/transition";
    import Signal  from "../signal";
    import { localStore} from './../localStore'

    let computadorasConectadas = window.estadisticas.computadorasConectadas;
    let rozCreados = window.estadisticas.hilosCreados;
    let comentariosCreados = window.estadisticas.comentariosCreados;
    let estabilidad = window.estadisticas.estabilidad;
    let estadisticasStore = localStore("Estadisticas", {visible: false});

    function formatear(numero) {
        if (numero >= 1000) return `${(numero / 1000).toFixed(2)}K`;
        return numero;
    }
    Signal.coneccion.on('estadisticasActualizadas', estadisticas => {
        computadorasConectadas = estadisticas.computadorasConectadas;
        rozCreados = estadisticas.hilosCreados;
        comentariosCreados = estadisticas.comentariosCreados;
        estabilidad = estadisticas.estabilidad;
    })
</script>

{#if $estadisticasStore.visible}
    <div
        class="container"
        transition:fly={{ opacity: 1, y: -48, duration:300 }}
        style="position:absolute; width:100vw;z-index: 1;height: 100%; pointer-events: none !important;">
        <div class="estadisticas " on:click={() => ($estadisticasStore.visible = false)}>
            <span
                style="right: 68%;"
                title="Computadoras conectadas">{formatear(computadorasConectadas)}</span>
            <span
                style="right: 50%;"
                title="Roz creados">{formatear(rozCreados)}</span>
            <span
                style="right: 32%;"
                title="Comentarios creados">{formatear(comentariosCreados)}</span>
            <span
                style="right: 16%"
                title="Estabilidad">{estabilidad.toFixed(1)}%</span>
        </div>
    </div>
{/if}

<div on:click={() => ($estadisticasStore.visible = true)} class="desplegar-estadisticas">
    <span class="fe fe-bar-chart-2" />
    <Ripple />
</div>

<style>
    .estadisticas {
        background: url(/imagenes/estadisticas.png);
        background-size: 100%;
        background-repeat: no-repeat;
        width: 559px;
        height: 37px;
        margin: 0 auto;
        position: relative;
        top: -2px;
        box-shadow: 0 0 6px 1px #00000085;
        pointer-events: visible;
    }
    .estadisticas span {
        position: absolute;
        top: 9px;
        background-color: #f3ec78;
        background-image: linear-gradient(180deg, #ffffff 45%, #040404);
        background-size: 100%;
        -webkit-background-clip: text;
        -moz-background-clip: text;
        background-clip: text;
        -webkit-text-fill-color: transparent;
        -moz-text-fill-color: transparent;
        font-weight: 600;
        cursor: initial;
    }
    .desplegar-estadisticas {
        margin: 0 auto;
        background: var(--color5);
        font-size: 11px;
        align-self: baseline;
        border-radius: 0 0 4px 4px;
        color: var(--color2);
        padding: 1px 2px;
    }

    @media(max-width:600px) {
        .estadisticas span { top:4px}
        .estadisticas {
            font-size: 10px;
            height: 24px;
            width: 100vw;
        }
    }
</style>
