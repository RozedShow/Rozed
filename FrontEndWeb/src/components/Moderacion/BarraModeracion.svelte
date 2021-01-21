<script>
    import {ButtonGroup , Button} from 'svelte-mui'
import globalStore from '../../globalStore';
</script>

<nav class="moderacion-nav" style="width:100%;margin:0 auto;">
    <ButtonGroup  color="var(--color7)" >
        <a href="/Moderacion"><Button outlined>Pagina principal</Button> </a>
        <a href="/Moderacion/Media"><Button outlined>Imagenes y videos</Button> </a>
        {#if $globalStore.usuario.esMod}
            <a href="/Moderacion/ListaDeUsuarios"><Button outlined>Usuarios y baneos</Button> </a>
            <a href="/Moderacion/EliminadosYDesactivados"><Button outlined>Eliminados y desactivados</Button> </a>
            <a href="/Moderacion/Historial"><Button outlined>Historial</Button> </a>
        {/if}
    
    </ButtonGroup >
</nav>
<style>
    .moderacion-nav {
        width: 100%;
        justify-content: center;
        display: flex;
        margin-bottom: 8px !important;
    }
</style>
