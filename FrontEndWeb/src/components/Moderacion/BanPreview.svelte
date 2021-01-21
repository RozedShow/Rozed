<script>
    import { MotivoDenuncia } from '../../enums';
    import { formatearTiempo, formatearTimeSpan}  from '../../helper'
    import Comentario from "../Comentarios/Comentario.svelte"
    import HiloPreviewMod from "../Moderacion/HiloPreviewMod.svelte";

    export let ban
</script>

<div class="ban">
    <p>Aclaracion: {ban.aclaracion ||" "}</p>
    <p>Motivo: { MotivoDenuncia.aString(ban.motivo)}</p>
    <p>Fecha: {formatearTiempo(ban.creacion)}</p>
    <p>Duracion: {formatearTimeSpan(ban.duracion)}</p>
    <p>Id del ban: {ban.id}</p>

    {#if ban.comentario} 
        <Comentario comentario={ban.comentario}/>
    {/if}
    {#if ban.hilo} 
        <HiloPreviewMod hilo={ban.hilo}/>
    {/if}
</div>

<style>
    .ban {
        background: var(--color5);
        padding: 4px 10px;
        border-radius: 4px;
    }
    p {
        margin: 4px;
    }
</style>