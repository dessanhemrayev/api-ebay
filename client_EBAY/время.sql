SELECT `product`.`id`,
    `product`.`title`,
    `product`.`price`,
    `product`.`url`,
    `product`.`category_product_id`
FROM `api_ebay`.`product`,`api_ebay`.`price_product`where `product`.`id`=`price_product`.`product_id` and `price_product`.`date`>'2019-12-01';

