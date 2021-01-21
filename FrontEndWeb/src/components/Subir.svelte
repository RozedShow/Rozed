<script>
    let oculta = true
    let scrollY = 0
    $:  if(scrollY > 1000) {
        oculta = false
    }else {
        oculta = true
    }
    function subir () {
        document.body.scrollTop = 0
        document.documentElement.scrollTop = 0
    }
</script>

<svelte:window   bind:scrollY={scrollY} ></svelte:window>
<div class:oculta class="subir" on:click={subir}>
    <icon class="fe fe-arrow-up" />
</div>

<style>
    .subir {
        position: fixed;
        bottom: 0;
        right: 10px;
        background: rgba(7, 7, 26, 0.185);
        padding: 4px;
        border-radius: 10px 10px 0 0;
        z-index: 999;
        transition: 0.2s;
        cursor: pointer;
        opacity: 0.5;
    }

    .subir:hover {
        background: rgba(7, 7, 26, 0.774);
        opacity: 1;
    }

    .oculta {
        transform: translateY(50px);
    }

    @media(max-width: 600px) {
        .subir {
            transform: scale(2);
        }
        .oculta {
        transform: translateY(50px);
    }
    }
</style>
