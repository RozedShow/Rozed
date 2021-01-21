<script>
    import { TipoAccion, MotivoDenuncia } from '../../enums';
    import { Button } from 'svelte-mui'
    import Comentario from '../Comentarios/Comentario.svelte'
    import HiloPreviewMod from '../Moderacion/HiloPreviewMod.svelte'
    import Denuncia from '../Denuncia.svelte'
    import {formatearTiempo, formatearTimeSpan} from '../../helper'
    import Dialogo from '../Dialogo.svelte'
    import RChanClient from '../../RChanClient';
    import Tiempo from '../Tiempo.svelte'
    import BarraModeracion from '../Moderacion/BarraModeracion.svelte';
    import BanPreview from '../Moderacion/BanPreview.svelte';
    
    const historial = window.model.acciones

    let dialogoDesban = false

    let filtro = {
        usuario: '',
        accion: ''
    }

    $: accionesFiltradas = historial.filter(a => {
            let fUsuario = !filtro.usuario || a.usuario.userName == filtro.usuario
            let fAccion = (filtro.accion === '') || a.tipo === filtro.accion
            return fUsuario && fAccion
    }) || filtro

    let mods = Array.from(new Set(historial.map(a => a.usuario.userName)))
</script>
<main>
    <BarraModeracion/>
    <h3 style="text-align:center;margin-bottom: 10px;">Ultimas acciones</h3>
    <div class="filtros" style="display: flex;width: fit-content;margin: 0 auto;align-items: baseline;">
        <span>Filtrar: </span>
        <select bind:value={filtro.usuario}>
            <option value={''}>Usuario</option>
            {#each mods as m}
                <option value={m}>{m}</option>
            {/each}
        </select>
        
        <select bind:value={filtro.accion}>
            <option value={''}>Accion</option>
            {#each Object.keys(TipoAccion) as a}
                <option value={TipoAccion[a]}>{a}</option>
            {/each}
        </select>
    </div>
    <ul>
        {#each accionesFiltradas as a (a.id)}
            <li class="accion">
                <span style="background: var(--color3);">
                    <Tiempo date={a.creacion}/>
                </span>
                <span style="background: var(--color6);">{a.usuario.userName}</span>
                <span style="background: var(--color5);">{TipoAccion.aString(a.tipo)}</span>
                {#if a.tipo == TipoAccion.CategoriaCambiada}
                    <span>{a.nota}</span>
                {/if}
                {#if a.hilo}
                    <a href="/Hilo/{a.hilo.id}">Roz
                        <div class="desplegable roz">
                            <HiloPreviewMod hilo={a.hilo}/>
                        </div>
                    </a>
                {/if}
                {#if a.comentario}
                    <a href="/Hilo/{a.comentario.hiloId}#{a.comentario.id}">
                        Comentario
                        <div class="desplegable">
                            <Comentario comentario={a.comentario}/>
                        </div>
                    </a>
                {/if}
                {#if a.denuncia}
                    <a href="#">
                        Denuncia
                        <div class="desplegable">
                            <Denuncia denuncia = {a.denuncia}></Denuncia>
                        </div>
                    </a>
                {/if}
                {#if a.ban}
                    <a href="#" style="background: var(--color5);">
                        Ban
                        <div class="desplegable">
                            <BanPreview ban={a.ban}/>
                        </div>
                        <Dialogo textoActivador="Desbanear" titulo="Desbanear gordo" accion = {() => RChanClient.removerBan(a.ban.id)}>
                            <span slot="activador">desbanear</span>
                            <div slot="body">
                                Remover ban?
                            </div>
                        </Dialogo>
                    </a>
                {/if}
            </li>
        {/each}
    </ul>
</main>

<style>
    .desplegable {
        display: none;
        z-index: 99;
    }
    a {
        position: relative;
    }
    a:hover .desplegable, a:active .desplegable {
        display: block;
        max-width: 600px;
        width: max-content;
        position: absolute;
        top: 15px;
        left: 20px;
    }
    .roz {
        width: 300px !important;
        height: 300px !important;
    }

    ul {
        display: flex;
        flex-direction: column;
        width: fit-content;
        gap:10px;
        margin:auto
    }
    li {
        gap: 0;
    }
    .accion > *{
        padding: 4px 10px;
        display: block;
        min-width: 57px;
    }
    .accion {
        background: var(--color1);
        border-radius: 4px;
        display:flex;
        flex-direction: row;
        gap: 4px;
        background: var(--color7);
    }
</style>