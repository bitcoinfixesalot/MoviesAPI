SELECT top 5 M.Id, M.Genres, M.RunningTime, M.Title, M.YearOfRelease, Sum(R.RatingValue) AS SUMRATE FROM movies AS M
inner join Rating AS R ON R.MovieID = M.Id
GROUP BY M.Id, M.Genres, M.RunningTime, M.Title, M.YearOfRelease
ORder by SUMRATE DESC