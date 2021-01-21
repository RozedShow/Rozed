<script>
    import { formatearTimeSpan } from '../../helper';
    import Tiempo from '../Tiempo.svelte'
    import Dialogo from '../Dialogo.svelte'
    import RChanClient from '../../RChanClient'
    import BarraModeracion from '../Moderacion/BarraModeracion.svelte';
    let ultimosRegistros = window.model.ultimosRegistros
    let ultimosBaneos = window.model.ultimosBaneos
    let cantidadDeUsuarios = window.model.cantidadDeUsuarios
</script>

<BarraModeracion/>
<section>
    <div class="lista-baneos panel">
        <h2>Baneos activos</h2>
        <ul>
            {#each ultimosBaneos as b}    
            <li class="ban" >
                <a 
                style="color:var(--color6)" 
                    href="/Moderacion/HistorialDeUsuario/{b.usuario.id}">
                    {b.usuario.userName}</a>
                    fue baneado hace <Tiempo date={b.creacion}/> <br>
                    <a style="color:var(--color6)" href="/Hilo/{b.hiloId}#{b.comentarioId}">roz/comentario del baneo</a>
                    Duracion: {formatearTimeSpan(b.duracion)}
                    <br>
                    <Dialogo textoActivador="Desbanear" titulo="Desbanear gordo" accion = {() => RChanClient.removerBan(b.id)}>
                        <div slot="body">
                            Remover ban?
                        </div>
                    </Dialogo>
                </li>
                {/each}
            </ul>
        </div>
    <div class="lista-usuarios panel">
        <h2>Ultimos 100 usuarios registrados</h2>
        <h4>Hay un total de {cantidadDeUsuarios} usuarios registrados</h4>
        <br>
        <ul>
            {#each ultimosRegistros as u}    
                <li style="padding:4px 8px">Se registro 
                    <a style="color:var(--color6)" href="/Moderacion/HistorialDeUsuario/{u.id}">{u.userName}</a>  
                    hace <Tiempo date={u.creacion} />
                </li>
            {/each}
        </ul>
    </div>
</section>

<style>
    .ban {
        padding:4px 8px;
        background: var(--color4);
        margin-bottom: 4px;
        border-radius: 4px;
    }

    section {
        display: flex;
        gap: 10px;
        justify-content: center;
        align-items: flex-start;
    }

    section>div {
        max-width: 500px;
    }
</style>