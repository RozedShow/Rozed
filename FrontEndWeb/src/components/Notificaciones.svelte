<script>
    import RChanClient from '../RChanClient'
    import { fade, blur, fly } from 'svelte/transition';
    import {Ripple, Button} from 'svelte-mui'
    import Signal from '../signal';

    export let notificaciones

    const titulo = document.title

    $: totalNotificaciones = notificaciones.map(n => n.conteo).reduce((c, n) => c+=n, 0)
    let mostrar =  false
    let nuevasNotificaciones = []

    async function limpiar() {
        try {
            await RChanClient.limpiarNotificaciones()
        } catch (error) {
            console.log(error);
            return
        }
        notificaciones = []
        mostrar = false
    }

    Signal.coneccion.on("NuevaNotificacion", noti => {
        nuevasNotificaciones = [noti, ...nuevasNotificaciones]
        setTimeout(() => {
            nuevasNotificaciones.pop()
            nuevasNotificaciones = nuevasNotificaciones
        }, 3000 + nuevasNotificaciones.length * 1000)
        let yaExisteUnaNotiDeEseTipo = false
        let notiVieja = null
        for (const n of notificaciones) {
            if(n.hiloId == noti.hiloId && n.tipo == noti.tipo  && n.tipo == 0)
            {
                n.conteo++;
                yaExisteUnaNotiDeEseTipo = true
                notiVieja = n
            }
            else if(n.hiloId == noti.hiloId && n.comentarioId == noti.comentarioId && n.tipo == noti.tipo  && n.tipo == 1)
            {
                n.conteo++;
                yaExisteUnaNotiDeEseTipo = true
                notiVieja = n
            }
        }
        if(!yaExisteUnaNotiDeEseTipo) {
            noti.conteo = 1
            notificaciones = [noti, ...notificaciones]
        }else {
            notificaciones = notificaciones.filter(n => n != notiVieja)
            notificaciones = [notiVieja, ...notificaciones]
        }
    })

    Signal.coneccion.on("notificacionesLimpeadas", () => {
        notificaciones = []
    })
    // signal.coneccion.start().then(() => {
    //     console.log("Conectadito");
    //     return connection.invoke("SubscribirseAHilo", hilo.id)
        
    // }).catch(console.error)

</script>

<svelte:head>
	<title>{totalNotificaciones != 0?`(${totalNotificaciones})`:''} {titulo}</title>
</svelte:head>

<span style="position: relative;" >
    <Button icon dense
        on:click={() => mostrar = !mostrar && totalNotificaciones != 0}
    >
    <span class="fe fe-bell" ></span>
    {#if notificaciones.length != 0}
        <div class="noti-cont" style="position: absolute;">
            <span>{totalNotificaciones}</span>
        </div>
    {/if}
    <Ripple/>
    </Button>
    {#if mostrar}
        <ul transition:fly={{x: -50, duration:150}} class="notis panel drop-menu abs lista-nav menu1"
            on:mouseleave={() => mostrar = false}
        >
            {#each notificaciones as n}
                <a href="/Notificacion/{n.id}?hiloId={n.hiloId}&comentarioId={n.comentarioId}">
                    <li class="noti">
                        <img src="{n.hiloImagen}" alt="">
                        {#if n.tipo == 0}
                            <span>{n.conteo} Nuevos Comentarios en : {n.hiloTitulo}</span>
                        {:else}
                            <span>{n.conteo} Respondieron a tu comentario : {n.comentarioId}</span>
                        {/if}
                    </li>
                </a>
            {/each}
            <li class="noti" style="justify-content: center;"
                on:click={limpiar}
            >Limpiar todas</li>
        </ul>
    {/if}
</span>

<ul class="nuevas-notificaciones notis panel drop-menu abs lista-nav menu1">
    {#each nuevasNotificaciones as n}
        <div out:fly|local={{x: -150, duration:250}} >

            <a  href="/Notificacion/{n.id}?hiloId={n.hiloId}&comentarioId={n.comentarioId}">
                <li  class="noti">
                    <img src="{n.hiloImagen}" alt="">
                    <div class="">
                        <h3>{n.hiloTitulo}</h3>
    
                        {#if n.tipo == 0}
                            <span style="color: var(--color5)"> Han comentado </span>
                        {:else}
                            <span style="color: var(--color5)"> Han respondido tu comentario</span>
                        {/if}
                        <span>{@html n.contenido}</span>
                    </div>
                </li>
            </a>
        </div>
    {/each}
</ul>

<style>

.nuevas-notificaciones {
    position: fixed;
    bottom: 16px;
    top: auto;
    left: 16px;
    min-width: 320px;
    width: fit-content;
    z-index: 999999;
}
.nuevas-notificaciones li {
    max-width: 400px;
    max-height: 100px;
    overflow: hidden;
    border-top: 1px solid var(--color5);
    border-radius: 4px;
    margin-bottom: 8px;
}
.nuevas-notificaciones li h3{
    max-height: 30px;

}
</style>