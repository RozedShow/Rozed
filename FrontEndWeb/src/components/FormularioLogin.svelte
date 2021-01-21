<script>
    import { Dialog, Textfield, Button } from 'svelte-mui';
    import globalStore from '../globalStore'
    import RChanClient from '../RChanClient'
    import ErrorValidacion from './ErrorValidacion.svelte';

    export let nick = '';
    export let contraseña = '';
    let error = null

    async function registrar() {
        try {
            await RChanClient.registrase(nick, contraseña)
        } catch (e) {
            console.log(e);
            error = e.response.data
            return
        }
        window.location = "/"
        location.reload();
    }
    async function login() {
        try {
            await RChanClient.logearse(nick, contraseña)
        } catch (e) {
            console.log(e);
            console.log(e.response)
            error = e.response.data
            return
        }
        window.location = "/"
        location.reload();
    }

</script>
<Dialog width="290" bind:visible= {$globalStore.mostrarLogin}>
        <div slot="title"> Ingresar</div>
        
        <Textfield
        name="Nick"
        autocomplete="off"
        required
        bind:value={nick}
        label="nick"
        message="Como te llamas tu?"
        />
        <Textfield
        type="password"
        name="Contraseña"
        autocomplete="off"
        required
        bind:value={contraseña}
        label="Contraseña"
        message="Gordo1234"
        />
        
        <ErrorValidacion {error}/>
        <div slot="actions" class="actions center">
            <Button color="primary" on:click={login} >Jeje ta bien</Button>
        </div>
        
    </Dialog>

<Dialog width="290" bind:visible= {$globalStore.mostrarRegistro}>
    <div slot="title"> Registrate</div>

    <Textfield
        name="Nick"
        autocomplete="off"
        required
        bind:value={nick}
        label="nick"
        message="Como te llamas tu?"
    />
    <Textfield
        type="password"
        name="Contraseña"
        autocomplete="off"
        required
        bind:value={contraseña}
        label="Contraseña"
        message="Gordo1234"
    />
    <ErrorValidacion {error}/>

    <div slot="actions" class="actions center">
        <Button  on:click={registrar} color="primary" >Jeje ta bien</Button>
    </div>

</Dialog>

