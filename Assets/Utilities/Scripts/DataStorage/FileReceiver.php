<?php
    $filename = $_GET["filename"]; 
    $file = $_GET["file"];

    $result = file_put_contents("UAberta/" . $filename, $file);

    if ($result === FALSE) {
        $result = -1;
    }
    echo $result;
?>