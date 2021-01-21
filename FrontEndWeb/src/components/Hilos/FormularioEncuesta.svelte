<script>
    import {Checkbox, Dialog, Button, Ripple} from 'svelte-mui'
    let visible = true
    let estado = 0 // 0 sin encuesta, 1 agregando encuesnta 2 encuesta agregada
    
    const limiteOpciones = 6
    export let   opciones = new Set([])
    let opcionesArray = []
    let opcionNueva = ""

 

    function agregarOpcion() {
        if(opciones.size >= 6) return;
        opciones.add(opcionNueva)
        opciones = opciones
        opcionNueva = "" 
        opcionesArray = [...opciones]
    }
    function remover(opcion) {
        opciones.delete(opcion)
        opciones = opciones
        opcionesArray = [...opciones]
    }

    function cancelar() {
        opciones = new Set()
        estado = 0
        opcionesArray = [...opciones]
    }

    function aceptar() {
        estado = 2
        opciones = opciones
        opcionesArray = [...opciones]
        console.log(opcionesArray);
    }
</script>

<!-- <Checkbox bind:checked={visible}>Encuesta</Checkbox> -->
<span style="margin:4px 0;display: flex;justify-content: center;">

    {#if estado == 0  || estado == 1}
        <Button on:click={() => estado = 1}><icon class="fe fe-bar-chart-2" style="font-size: 1.2rem;"></icon></Button>
    {/if}
    {#if estado == 2}
        <Button color="var(--color5)" on:click={cancelar}><icon class="fe fe-bar-chart-2" style="font-size: 1.2rem;"></icon></Button>
    {/if}
</span>

{#if estado == 1}
    <Dialog visible={true}>
        <div slot="title">Opciones ({opciones.size}/{limiteOpciones})</div>
        <ul>
            {#each opcionesArray as o}
                <li on:click={() => remover(o)}>{o} <Ripple/></li>
            {/each}
        </ul>
        {#if opciones.size < 6}
            <input bind:value={opcionNueva} type="text" placeholder="Añadir opcion">
        {/if}
        <div style="margin-top:8px">
            {#if opciones.size > 1}
                <Button on:click={aceptar} color="primary">Aceptar</Button>
            {:else}
                <Button color="primary" on:click={cancelar}>Cancelar</Button>
            {/if}
            {#if opciones.size < 6}
                <Button color="primary" on:click={agregarOpcion}>Añadir</Button>
            {/if}
        </div>
    </Dialog>
{/if}

<style>
    li {
        user-select: none;
        cursor: pointer;
        padding: 10p;
        padding: 8px 4px;
        border-bottom: dashed 1px var(--color7);
    }

    li:hover {
        background-color: rgba(255, 0, 0, 0.308);
    }
</style>

