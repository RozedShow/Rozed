<script>
    export let cargando = false
</script>

{#if cargando}
    <div class="loader">
        <span>R</span>
    </div>
    {:else}
    <slot></slot>

{/if}

<style>
.loader {
    border: 2px solid var(--color4); 
    border-top: 2px solid var(--color5); 
    /* border-radius: 50%; */
    width: 24px;
    height: 24px;
    animation: spin 2s linear infinite;
    align-self: center;
    font-family: eurofighter;
    color: white;
    background: var(--color2);
}

span {
    position: relative;
    left: 4px;
    top: 4px;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
</style>

