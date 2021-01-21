<script>
    import Media from "../Media.svelte";
    export let hilo = null

    let mediaExpandido= false

</script>

<div class="cuerpo" class:mediaExpandido>
    <Media media={hilo.media} bind:abierto={mediaExpandido}/>
    <h1 style="margin-bottom:16px">{hilo.titulo}</h1>
    <div class="texto" style="white-space: pre-wrap;word-break: break-word;">{@html hilo.contenido}</div>
</div>

<style>
    .mediaExpandido {
        display: flex;
        flex-wrap: wrap;
    }

    .mediaExpandido h1, .mediaExpandido .texto {
        width: 100%;
    }

    :global(.media) {
        float: left;
        margin-right: 10px;
        max-width: 50%;
        overflow: hidden;
    }
    .cuerpo {
        padding: 0 10px;
        direction: rtl;
    }
    .cuerpo * {
        direction: ltr;
    }
    @media (max-width: 992px) {
	.cuerpo :global(.media) {
		max-width: 100%;
		width: 100% !important;
		float: none
	}
    .cuerpo :global(a) {
        color: var(--color5) !important;
    }

    @media (max-width: 600px) {
        h1{
            font-weight: bold !important;
        }
    }

}
</style>