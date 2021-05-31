SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;


CREATE TABLE `EX2_space_invaders_accounts` (
  `id` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `password` varchar(256) NOT NULL,
  `password_raw` varchar(128) NOT NULL,
  `is_admin` tinyint(1) NOT NULL DEFAULT '0',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_seen` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `EX2_space_invaders_accounts` (`id`, `name`, `password`, `password_raw`, `is_admin`, `created_at`, `last_seen`) VALUES
(3, 'natacha', '*4CEAD9E5BFCE723AACAC8EE02D027E7C1E66E712', 'nb123456', 1, '2021-05-03 14:56:23', '2021-05-03 14:56:23'),
(4, 'admin', '*E98140D5D5455ACE31C19A8D3D3C5A313714BC2B', 'admin', 1, '2021-05-03 14:59:41', '2021-05-17 13:03:31'),
(12, 'arthuro', '*CB0A8309B0B2C3D40A40CF5EA97C4F8430E17BAE', 'admin', 0, '2021-05-13 15:30:14', '2021-05-13 15:30:14'),
(13, 'a', '*0E09DF2FAF0F3A439B4788CAC316EC9D7F3B2B83', 'a', 0, '2021-05-27 11:47:51', '2021-05-29 12:10:58');

CREATE TABLE `EX2_space_invaders_scores` (
  `id` int(11) NOT NULL,
  `owner` int(11) NOT NULL,
  `score` int(11) NOT NULL,
  `registered_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `EX2_space_invaders_scores` (`id`, `owner`, `score`, `registered_at`) VALUES
(21, 12, 3830, '2021-05-13 15:35:04'),
(22, 4, 960, '2021-05-17 13:03:16'),
(23, 4, 960, '2021-05-17 13:03:17'),
(24, 13, 5950, '2021-05-29 12:00:54');


ALTER TABLE `EX2_space_invaders_accounts`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `EX2_space_invaders_scores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `owner` (`owner`);


ALTER TABLE `EX2_space_invaders_accounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;
ALTER TABLE `EX2_space_invaders_scores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

ALTER TABLE `EX2_space_invaders_scores`
  ADD CONSTRAINT `EX2_space_invaders_scores_ibfk_1` FOREIGN KEY (`owner`) REFERENCES `EX2_space_invaders_accounts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
