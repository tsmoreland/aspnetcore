async function setImageUsingStreaming(imageElementId, imageStream) {
  const arrayBuffer = await imageStream.arrayBuffer();
  const blob = new Blob([arrayBuffer]);
  const url = URL.createObjectURL(blob);

  const element = document.getElementById(imageElementId);
  if (element !== undefined && element !== null) {
    document.getElementById(imageElementId).src = url;
  } else {
    console.log(imageElementId + ' not found')
  }
} 
