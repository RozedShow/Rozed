<script>
    import {Button, Checkbox,Radio} from 'svelte-mui'
    import config from '../../config'
    import RChanClient from '../../RChanClient'
    import Dialogo from '../Dialogo.svelte'
    import ErrorValidacion from '../ErrorValidacion.svelte'

    let model = window.model
    let error = null
    let nickAdmin = ""
    let nickMod = ""

    let restAcc = 0;

    async function eliminar(nick, rol) {
        try {
            let res = await RChanClient.removerRol(nick, rol)
            console.log(res)
            alert(res.data.mensaje)
        } catch (e) {
            console.log(e.resposne)
            error = e.response.data
            return
        }
    }

    async function añadir(nick, rol) {
        try {
            let res = await RChanClient.añadirRol(nick, rol)
            console.log(res)
            alert(res.data.mensaje)
        } catch (e) {
            console.log(e.resposne)
            error = e.response.data
            return
        }
    }
    async function actualizarConfig() {
        try {
            let res = await RChanClient.ActualizarConfiguracion(model.config)
            console.log(res)
            alert(res.data.mensaje)
        } catch (e) {
            console.log(e.resposne)
            error = e.response.data
            return
        }
    }

    async function generarLink() {
        try {
            const res = await RChanClient.generarNuevoLinkDeInvitacion()
            model.config.linkDeInvitacion = res.data.link
            alert("Nuevo link generado")
        } catch (error) {
            
        }
    }

    let restriccionDeAcesso = 2;
    let restriccionesDeAcesso = {
        Libre: 0,
        Registrados: 1,
        Administradores:2,

    };

</script>
<main class="administracion">
    <section >
        <h3>Equipo</h3>
        <ErrorValidacion error={error}/>
        <div class="menu">
            <ul >
                <li class="header">Admninistradores</li>
                <li class="noback">
                    <input bind:value = {nickAdmin}  type="text" placeholder="Id o nick del usuario"> <Button on:click={() => añadir(nickAdmin, "admin")}>Añadir</Button>
                </li>
                {#each model.admins as a (a.id)}
                    <li>{a.userName} <span class="sep"></span><Button on:click={() => eliminar(a.id, "admin")}>Eliminar</Button></li>
                {/each}
                <hr>
                <li class="header">Moderadores(medz)</li>
                <li class="noback">
                    <input bind:value = {nickMod}  type="text" placeholder="Id o nick del usuario"> <Button on:click={() => añadir(nickMod, "mod")}>Añadir</Button>
                </li>
                {#each model.mods as m (m.id)}
                    <li>{m.userName} <span class="sep"></span><Button on:click={() => eliminar(m.id, "mod")}>Eliminar</Button></li>
                {/each}
                <li class="header">Auxiliares(aux)</li>
                <li class="noback">
                    <input bind:value = {nickMod}  type="text" placeholder="Id o nick del usuario"> <Button on:click={() => añadir(nickMod, "auxiliar")}>Añadir</Button>
                </li>
                {#each model.auxiliares as m (m.id)}
                    <li>{m.userName} <span class="sep"></span><Button on:click={() => eliminar(m.id, "auxiliar")}>Eliminar</Button></li>
                {/each}
            </ul>

        </div>
    </section>

    <section >
        <h3>Configuracion</h3>
        <ErrorValidacion error={error}/>
        <div class="menu">
            <ul >
                <li>Limite bump <input bind:value={model.config.limiteBump} type="number"></li>
                <li>Tiempo entre comentario <input bind:value={model.config.tiempoEntreComentarios} type="number"></li>
                <li>Tiempo entre hilos<input bind:value={model.config.tiempoEntreHilos} type="number"></li>
                <li>Hilos maximos por categoria<input bind:value={model.config.hilosMaximosPorCategoria} type="number"></li>
                <li>Limite archivo<input bind:value={model.config.limiteArchivo} type="number"></li>
                <li>Captcha registro <Checkbox bind:checked={model.config.captchaRegistro} right></Checkbox></li>
                <li>Captcha hilo <Checkbox bind:checked={model.config.captchaHilo} right></Checkbox></li>
                <li>Captcha comentario <Checkbox bind:checked={model.config.captchaComentario} right></Checkbox></li>
                <li class="header"> <span style="margin-right: auto"></span> <Button on:click={actualizarConfig}>Guardar</Button></li>
            </ul>
        </div>
    </section>
    <section >
        <h3>Registro</h3>
        <ErrorValidacion error={error}/>
        <div class="menu">
            <ul >
                <li>Registros maximos por ip<input bind:value={model.config.numeroMaximoDeCuentasPorIp} type="number"></li>
                <li>Registro publico <Checkbox bind:checked={model.config.registroAbierto} right></Checkbox></li>
             
                {#if !model.config.registroAbierto}
                    <li>
                            <h4>Link de invitacion</h4>
                    </li>
                    <li>
                        <a style="color:var(--color5)" href="/Registro?codigoDeInvitacion={model.config.linkDeInvitacion}">/Registro?codigoDeInvitacion={model.config.linkDeInvitacion}</a>

                    </li>
                    <Button on:click={generarLink}>Nuevo link</Button>
                {/if}
                <li class="header"> <span style="margin-right: auto"></span> <Button on:click={actualizarConfig}>Guardar</Button></li>
            </ul>
        </div>
    </section>
    <section >
        <h3>Acceso</h3>
        <ErrorValidacion error={error}/>
        <div class="menu">
            <ul >
                <li>Protocolo Messi<Checkbox bind:checked={model.config.modoMessi} right></Checkbox></li>
                <li>Protocolo Serenito<Checkbox bind:checked={model.config.modoSerenito} right></Checkbox></li>
                {#each Object.keys(restriccionesDeAcesso) as key}
                <li>
                    <Radio right bind:group={model.config.restriccionDeAcceso} value={restriccionesDeAcesso[key]}>
                        <span>{key}</span>
                    </Radio>
                </li>
                {/each}
                <li>Mensaje pagina de choque</li>
                <textarea bind:value={model.config.mensajePaginaDeChoque} cols="30" rows="10"></textarea>
                <li>
                    Flags: 
                    <input type="text" bind:value={model.config.flags}>
                </li>
                <li class="header"> <span style="margin-right: auto"></span> <Button on:click={actualizarConfig}>Guardar</Button></li>
            </ul>
        </div>
    </section>
    <section >
        <h3>Mantenimiento</h3>
        <ErrorValidacion error={error}/>
        <div class="menu">
            <ul >
               <Dialogo accion={() => RChanClient.limpiarRozesViejos()}
                titulo={'Limpiar rozes viejos'}
                textoActivador={'Limpiar rozes viejos'}>
                    <div slot="body">
                        Eliminar los rozs con mas de 48 horas de su  eliminados o archivacion
                    </div>
               </Dialogo>
            </ul>
        </div>
    </section>

</main>

<style>
    main {
        margin: auto;
        display: flex;
        flex-wrap: wrap;
        gap:10px;
        max-width: 1270px;
        margin:auto;
    }
    main > section {
        flex: 1;
    }
    section {
        max-width: 400px;
        width: max-content;
        min-width: 270px !important;

    }
    .header {
        background: rgba(255, 255, 255, 0.089) !important;
    }
    .sep{
        margin-left: auto;
    }
    
    textarea {
        padding: 0 16px;
        background: var(--color1);
    }
</style>