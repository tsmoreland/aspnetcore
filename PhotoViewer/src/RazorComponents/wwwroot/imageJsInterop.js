async function setImageUsingStreaming(elementId, imageStream) {
    const arrayBuffer = await imageStream.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    const element = document.getElementById(elementId);
    if (element !== undefined && element !== null) {
        element.style.backgroundImage = `url(${url})`
        element.style.backgroundSize = 'cover';
        element.style.height = '100%';
        element.style.flex = '1 1 auto';
        console.log(element.style.background)
    } else {
        console.log(imageElementId + ' not found')
    }
} 
