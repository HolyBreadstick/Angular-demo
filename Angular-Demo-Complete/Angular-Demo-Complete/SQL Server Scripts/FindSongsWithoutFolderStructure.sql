USE MusicDemo;

SELECT song.title, art.firstName FROM dbo.Songs song
LEFT JOIN dbo.Albums alb ON alb.ID = song.Owner_ID
LEFT JOIN dbo.Artists art ON art.ID = alb.Owner_ID
WHERE song.canDownload = 0
OR
song.FilePath IS NULL;
