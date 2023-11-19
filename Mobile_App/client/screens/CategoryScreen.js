import { useNavigation, useRoute } from '@react-navigation/native';
import React, { useEffect, useState } from 'react';
import { ScrollView, Text, TouchableOpacity, View } from 'react-native';
import * as Icon from 'react-native-feather';
import DishRow from '../components/dishRow';
import { themeColors } from '../theme';

const CategoryScreen = () => {
    const navigation = useNavigation();
    const { params: { id, title, resturants } } = useRoute();
    const [categoryDishes, setCategoryDishes] = useState([]);

    useEffect(() => {
        // Gọi API để lấy danh sách sản phẩm theo category ID từ `resturants`
        const fetchData = async () => {
            try {
                // Thay thế link API thật của bạn
                const response = await fetch(`https://7f72-14-191-242-235.ngrok-free.app/api/Category/${id}`);
                const data = await response.json();
                setCategoryDishes(data.dishes);
            } catch (error) {
                console.error('Error fetching category dishes:', error);
            }
        };

        fetchData();
    }, [id]);

    return (
        <>
            <ScrollView style={{ backgroundColor: '#fff' }}>
                <View style={{ padding: 20 }}>
                    <TouchableOpacity
                        onPress={() => navigation.goBack()}
                        style={{ marginBottom: 20 }}
                    >
                        <Icon.ArrowLeft strokeWidth={3} stroke={themeColors.bgColor(1)} />
                    </TouchableOpacity>
                    <Text style={{ fontSize: 24, fontWeight: 'bold', marginBottom: 10 }}>{title}</Text>
                    {/* Loop through categoryDishes and display them */}
                    {categoryDishes.map(dish => (
                        <DishRow
                            key={dish._id}
                            id={dish._id}
                            name={dish.name}
                            description={dish.description}
                            price={dish.price}
                            image={dish.image}
                        />
                    ))}
                </View>
            </ScrollView>
        </>
    );
};

export default CategoryScreen;
