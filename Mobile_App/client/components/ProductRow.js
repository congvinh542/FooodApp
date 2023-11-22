import React from 'react';
import { Image, Text, TouchableOpacity, View } from 'react-native';
import * as Icon from "react-native-feather";
import { useDispatch, useSelector } from 'react-redux';
import { addToBasket, removeFromBasket, selectBasketItemsById } from '../slices/basketSlice';
import { themeColors } from '../theme';

const ProductRow = ({ name, description, id, price, pathImage, isLastItem, quantity }) => {
  const dispatch = useDispatch();
  const basketItems = useSelector(state => selectBasketItemsById(state, id));

  const handleIncrease = () => {
    dispatch(addToBasket({ id, name, price, pathImage, quantity, description }));
  };

  const handleDecrease = () => {
    dispatch(removeFromBasket({ id }));
  };

  return (
    <View style={{ 
      flexDirection: 'row', 
      alignItems: 'center', 
      backgroundColor: 'white', 
      padding: 16, borderRadius: 12, 
      shadowColor: '#000', 
      shadowOffset: { width: 0, height: 2 }, 
      shadowOpacity: 0.25, 
      shadowRadius: 3.84, 
      elevation: 5, 
      marginVertical: 8, 
      marginHorizontal: 16,
      marginBottom: isLastItem ? 100 : 0,
      }}>
      <Image className="rounded-3xl" style={{ height: 100, width: 100 }} source={{
        uri: pathImage
      }} />
      <View style={{ flex: 1, marginLeft: 16 }}>
        <Text style={{ fontSize: 20, fontWeight: 'bold' }}>{name}</Text>
        <Text style={{ color: '#888', marginTop: 4, fontWeight: 'bold' }}>Số lượng: {quantity}</Text>
        <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginTop: 8 }}>
          <Text className="text-gray-700 text-lg font-bold">{formatCurrency(price)} VNĐ</Text>
          <View style={{ flexDirection: 'row', alignItems: 'center' }}>
            <TouchableOpacity
              onPress={handleDecrease}
              disabled={!basketItems.length}
              style={{ padding: 8, borderRadius: 50, backgroundColor: themeColors.bgColor(1), marginRight: 8 }}>
              <Icon.Minus strokeWidth={2} height={20} width={20} stroke="white" />
            </TouchableOpacity>
            <Text style={{ paddingHorizontal: 8 }}>{basketItems.length}</Text>
            <TouchableOpacity
              onPress={handleIncrease}
              style={{ padding: 8, borderRadius: 50, backgroundColor: themeColors.bgColor(1) }}>
              <Icon.Plus strokeWidth={2} height={20} width={20} stroke="white" />
            </TouchableOpacity>
          </View>
        </View>
      </View>
    </View>
  );
};
const formatCurrency = (amount) => {
  const formattedAmount = amount.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
  return formattedAmount.endsWith('.00') ? formattedAmount.slice(0, -3) : formattedAmount;
};
export default ProductRow;
