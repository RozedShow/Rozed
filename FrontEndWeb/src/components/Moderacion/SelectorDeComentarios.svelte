
<script>
    import selectorStore from './selectorStore'
    import { Button } from 'svelte-mui'
    import { abrir } from '../Dialogos/Dialogos.svelte';

    function eliminarComentariosSeleccionados() {
        abrir.eliminarComentarios(Array.from($selectorStore.seleccionados))
        $selectorStore.seleccionados = new Set([])
    }


</script>

{#if $selectorStore.seleccionados.size != 0}
    <div class="selector-comentarios">
        <Button on:click={eliminarComentariosSeleccionados} color="var(--color5)" raised>Eliminar {$selectorStore.seleccionados.size} Comentario/s </Button>
    </div>
{/if}

<style>
    .selector-comentarios {
        position: fixed;
        right: 10px;
        bottom: 60px;

        z-index: 10;
    }
</style>

