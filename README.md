# 📖 ContrarianReads (Backend repo)

![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![Angular Material](https://img.shields.io/badge/Angular%20Material-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![RxJS](https://img.shields.io/badge/RxJS-B7178C?style=for-the-badge&logo=reactivex&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![AutoMapper](https://img.shields.io/badge/AutoMapper-ED1C24?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=database&logoColor=white)

ContrarianReads is a crowdsourced platform that helps readers explore diverse perspectives through book recommendations. When users input a non-fiction book, the platform generates recommendations for other non-fiction works that present *opposing viewpoints* or alternative arguments. Community members can submit book suggestions and vote on recommendations, helping surface the most relevant counterpoint books for each title.

The platform is designed to combat the ["filter bubble"](https://www.wikiwand.com/en/articles/Filter_bubble) effect—where algorithms and personal choices limit exposure to differing viewpoints—by actively encouraging readers to explore perspectives that challenge their existing beliefs. By facilitating access to well-reasoned opposing arguments, ContrarianReads helps users broaden their intellectual horizons and develop a more nuanced understanding of complex topics.

<figure>
  <img src="./screenshots/db-diagram.png" alt="Database diagram">
  <figcaption>Database ERD diagram</figcaption>
</figure>

# 🌐 Live demo

Work in progress. Will be soon available online.

# 🚦 Deploy status

Work in progress. Will be soon available online.

# 🛠️ Technologies used

## 🎨 Frontend

- Built with **Angular** and styled using **Angular Material**
- Allows logged-in users to search for books, submit, upvote and comment alternative recommendations
- Secure login and register system implementing **JWT authentication**
- User profile and user activity dashboard
- Alternative book recommendations based on user activity
- Hosted on **Netlify**
- Find the frontend repo [here](https://github.com/dividedby-0/contrarian-reads-frontend)

## ⚙️ Backend

- Developed with **ASP.NET Core**
- Provides secure APIs for authentication using **JWT**
- Books/recommendations/comments/upvotes management through secure RESTful endpoints
- Password encryption using **BCrypt**
- Data stored in **Microsoft SQL Server** database
- Backend and database hosted on **Microsoft Azure**

# 🚀 Future plans

- [ ] (WIP)

# 📝 License

This project is licensed under the [MIT License](https://github.com/dividedby-0/contrarian-reads-frontend/blob/main/LICENSE). Logos from [SVGRepo](https://www.svgrepo.com/).

## 🤝 Contributing

While this is primarily a personal project, bug reports and feature suggestions are welcome in the issues section.
