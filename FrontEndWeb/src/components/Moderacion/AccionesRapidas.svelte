<script>
    import config from '../../config';
    import RChanClient from '../../RChanClient'

    function crearAccion(nombre, nombreCorto, accion, atajo="", icon="") {
        return   {
            nombre,
            nombreCorto,
            atajo,
            icon,
            accion,
        }
    }

    function cambioDeCategoria(categoria) {
        return (hiloId) => {
            return RChanClient.cambiarCategoria(hiloId, categoria)
        }
    }

    const accionesRapidas = [
        crearAccion("Abortar Roz", "AR", RChanClient.borrarHilo, "1", "https://static.wikia.nocookie.net/ageofempires/images/8/8c/Blastfurnace.jpg/revision/latest/smart/width/40/height/30?cb=20190904083839"),
        crearAccion("A ay no se", "AYNS", cambioDeCategoria(2), "2", "https://static.wikia.nocookie.net/ageofempires/images/8/8c/Blastfurnace.jpg/revision/latest/smart/width/40/height/30?cb=20190904083839"),
    ]

    export let accionActiva = null
    
</script>

<div class="acciones-rapidas">
    <ul>
        {#each accionesRapidas as a (a.nombre)}
            <li class:activa={accionActiva == a} on:click={() => accionActiva = a} class="accion-rapida" style="background:url({a.icon})" title={a.nombre}>
                <span>
                    {a.nombreCorto}
                </span>
            </li>
        {/each}
    </ul>
</div>

<style>
    .acciones-rapidas {
        position: fixed;
        left: 0;
        top: 128px;
        display: flex;
        flex-direction: column;
        gap: 10px;
        background: rgba(0, 0, 0, 0.678);
        padding: 10px;
        z-index: 10;
    }
    .accion-rapida {
        border-radius: 4px;
        background: var(--color5);
        width: 32px;
        height: 32px;
        margin-bottom: 4px;
        font-size: 10px;
        text-align: center;
        display: flex;
        align-content: center;
        align-items: center;
        justify-content: center;
    }
    .accion-rapida span {
        background: linear-gradient(90deg, black, transparent);
        font-size: 13px;
        width: 100%;
        padding: 2px;
    }
    .activa {
        border: dashed 2px var(--color5);
    }
</style>