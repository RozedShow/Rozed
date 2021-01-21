<script> 
    import { Menu, Ripple, Button, Icon } from 'svelte-mui'
    import {fly} from "svelte/transition"

    export let mostrarMenu = false

</script>

<span on:click={() => mostrarMenu = true}>
    <slot name="activador"></slot>
</span>
{#if mostrarMenu}
    <ul class="menu" in:fly|local={{x:-100}} out:fly|local={{x:100}} on:mouseleave={() => mostrarMenu = false} >
        <slot></slot>
    </ul>
{/if}

<style>
.menu {
    margin: 0;
    list-style: none;
    padding: 0;
    background: var(--color2) !important;
    border-radius: 4px;
    position: absolute;
    right: 0;
    min-width: 150px;
    z-index: 99999;
}

.menu :global(li) {
    position: relative;
    cursor: pointer;
    height: 44px;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    display: flex;
    align-items: center;
    padding: 0 16px;
    white-space: nowrap;
}

</style>