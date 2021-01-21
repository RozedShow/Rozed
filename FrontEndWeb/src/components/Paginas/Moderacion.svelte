<script>
    import Comentario from '../Comentarios/Comentario.svelte'
    import { fly } from 'svelte/transition';
    import Denuncia from '../Denuncia.svelte'
    import HiloPreview from '../Hilos/HiloPreview.svelte'
    import BarraModeracion from '../Moderacion/BarraModeracion.svelte';
    import ComentarioMod from '../Moderacion/ComentarioMod.svelte';
    import HiloPreviewMod from '../Moderacion/HiloPreviewMod.svelte';
    import Sigal from '../../signal'

    let hilos = window.model.hilos
    let comentarios = window.model.comentarios
    let denuncias = window.model.denuncias
    let comentariosMedia = window.model.medias

    comentarios = comentarios.map (c => {
        c.respuestas = []
        return c
    })

    Sigal.subscribirAModeracion()
    Sigal.coneccion.on("NuevoComentarioMod", comentario => {
        comentario.respuestas = []
        comentarios.unshift(comentario)
        comentarios.pop()
        comentarios = comentarios
    })
    Sigal.coneccion.on("HiloCreadoMod", hilo => {
        hilos.pop()
        hilos = [hilo, ...hilos]
    })

</script>
<main>
    <BarraModeracion/>
    <div class="ultimos-medias">
        <ul>
            {#each comentariosMedia as c}
                <li>
                    <a href="/Hilo/{c.hiloId}#{c.id}">
                        <img src="{c.media.vistaPreviaCuadrado}" alt="">
                    </a>
                </li>
            {/each}
        </ul>
    </div>
    <div class="seccion2">
        <ul style="width:33%; background:#711c08;        font-size: 0.7em;    ">
            <h3 style="height:40px">Ultimas denuncias</h3>
            {#each denuncias as d}
                <Denuncia denuncia={d}/>
            {/each}
        </ul>
        <ul style="width:33%">
            <h3 style="height:40px">Ultimos hilos</h3>
            {#each hilos as h (h.id)}
                <HiloPreviewMod hilo={h}/>
            {/each}
        </ul>
        <ul>
            <h3 style="height:40px">Ultimos comentarios</h3>
            {#each comentarios as c}
                <li transition:fly|local={{y: -50, duration:250}}>
                    <ComentarioMod comentario={c}/>
                </li>
            {/each}
        </ul>
    </div>
</main>

<style >
    main {
        display: flex;
        gap: 10px;
        margin: auto;
        justify-content: center;
        align-items: center;
        flex-direction: column;
    }

    .seccion2 {
        width: 100%;
        max-width: 1800px;
        display: flex;
        gap: 10px;
        margin:auto;
        justify-content: center;
    }
    .seccion2 ul {
        max-width: 500px;
        background: var(--color2);
        padding: 10px
    }

    .ultimos-medias ul {
        display: flex;
        flex-wrap: wrap;
        gap: 4px;
    }
    .ultimos-medias img {
        width: 64px;
        height: 64px;
        border-radius: 4px;
    }

</style>